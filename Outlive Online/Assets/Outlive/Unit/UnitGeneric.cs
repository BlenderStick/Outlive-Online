using System.Collections;
using System.Collections.Generic;
using Outlive.Unit.Interacts;
using Outlive.Unit.Command;
using Outlive.Unit.Generic;
using UnityEngine;

namespace Outlive.Unit
{
    public class UnitGeneric : MonoBehaviour, ICommandableUnit, IGUIUnit, ISelectableUnit
    {

        [SerializeField] private string guiNameField;
        [SerializeField] private Player playerField;
        [SerializeField] private BasicBehaviour[] behaviours;
        [SerializeField] private IUnitInteract[] interacts;

        private IList<ICommand> commands;
        private Object commandsLock;
        private ICommand currentCommand;
        private IBehaviour currentBehaviour;

        // [SerializeField]
        public string guiName 
        {
            get
            {
                return guiNameField;
            }
        }

        public Player player 
        {
            get
            {
                return playerField;
            }
            set
            {
                playerField = value;
            }
        }

        public void GUILostFocus()
        {}

        public void GUIReceivedFocus()
        {}

        public void PutCommand(ICommand command, bool enfilerate)
        {
            lock (commandsLock)
            {
                if(currentCommand == null)
                {
                    currentCommand = command;
                            
                    if (!TryFindBehaviour(command, out currentBehaviour))
                        Debug.Log("Não há um comportamento que executa o comando recebido " + command.GetType().Name);
                    else
                        currentBehaviour.Setup(gameObject);
                }
                else
                {
                    if(enfilerate)
                    {
                        commands.Add(command);
                    }
                    else
                    {
                        if(currentBehaviour != null)
                        {
                            currentBehaviour.Cancel(gameObject, currentCommand);
                            currentBehaviour.Reset();
                        }
                        currentCommand = command;
                        
                        if (!TryFindBehaviour(command, out currentBehaviour))
                            Debug.Log("Não há um comportamento que executa o comando recebido " + command.GetType().Name);
                        else
                            currentBehaviour.Setup(gameObject);
                    }
                }
            }
            
        }

        public void PutInteract(GameObject target, bool enfilerate)
        {
            lock (commandsLock)
            {
                // ICommand command;

                // foreach (IUnitInteract i in interacts)
                // {
                //     if (i.Interact(gameObject, target, out comm))
                // }

                if(currentCommand == null)
                {
                    if (!TryFindInteract(target, out currentCommand, out currentBehaviour))
                        Debug.Log("Não há uma interação adequada para que " + gameObject.name + " tenha um comportamento para " + target.name);
                }
                else
                {
                    if(enfilerate)
                    {
                        commands.Add(new InteractCommand(target));
                    }
                    else
                    {
                        if(currentBehaviour != null)
                        {
                            currentBehaviour.Cancel(gameObject, currentCommand);
                            currentBehaviour.Reset();
                        }
                        
                        if (!TryFindInteract(target, out currentCommand, out currentBehaviour))
                            Debug.Log("Não há uma interação adequada para que " + gameObject.name + " tenha um comportamento para " + target.name);
                    }
                }
            }
        }

        private IBehaviour FindBehaviour(ICommand command)
        {
            foreach (IBehaviour b in behaviours)
            {
                if (b.Condition(command))
                    return b;
            }
            return null;
        }
        private bool TryFindBehaviour(ICommand command, out IBehaviour behaviour)
        {
            foreach (IBehaviour b in behaviours)
            {
                if (b.Condition(command))
                {
                    behaviour = b;
                    return true;
                }
            }
            behaviour = null;
            return false;
        }

        private bool TryFindInteract(GameObject target, out ICommand command, out IBehaviour behaviour)
        {
            IBehaviour b;
            ICommand c;
            foreach (IUnitInteract i in interacts)
            {
                if (i.Interact(gameObject, target, out b) && i.Command(gameObject, target, out c))
                {
                    command = c;
                    behaviour = b;
                    return true;
                }
            }
            command = null;
            behaviour = null;
            return false;
        }

        #region ISelectableUnit
            [SerializeField] private Light selection;
            [SerializeField] private float selectedIntense;
            [SerializeField] private float hoverIntense;
            [SerializeField] private float deselectedIntense;

            private bool selected;
            public void UnidDeselect()
            {
                selection.intensity = deselectedIntense;
                selected = false;
            }

            public void UnitSelect()
            {
                selection.intensity = selectedIntense;
                selected = true;
            }
            public void UnitHover()
            {
                if (selected)
                    selection.intensity = selectedIntense + hoverIntense;
                else
                    selection.intensity = hoverIntense;
            }

            public void UnitNotHover()
            {
                if(selected)
                    selection.intensity = selectedIntense;
                else
                    selection.intensity = deselectedIntense;
            }
        #endregion
        

        public void UpdateCommand()
        {
            if (currentBehaviour != null)
            {
                lock (commandsLock)
                {
                    if (!currentBehaviour.UpdateBehaviour(gameObject, currentCommand))
                    {
                        currentBehaviour.Reset();
                        currentBehaviour = null;
                    }
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            commandsLock = new Object();
            if (player != null && selection != null)
            {
                selection.color = player.color;
            }
            if (gameObject != null)
            FindObjectOfType<PlayerManager>().InstallUnitsInScene(base.gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            UpdateCommand();
        }

    }
}