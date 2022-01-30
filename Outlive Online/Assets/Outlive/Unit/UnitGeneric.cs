using System.Collections;
using System.Collections.Generic;
using Outlive.Unit.Interacts;
using Outlive.Unit.Command;
using Outlive.Unit.Generic;
using UnityEngine;
using Outlive.Manager.Generic;

namespace Outlive.Unit
{
    [AddComponentMenu("Outlive/Unit/Unit"), RequireComponent(typeof(UnitStarter))]
    public class UnitGeneric : MonoBehaviour, ICommandableUnit, IGUIUnit, ISelectableUnit
    {

#pragma warning disable 0649
        [SerializeField] private string _guiName;
        [SerializeField] private IPlayer _player;
        [SerializeField] private BasicBehaviour[] _behaviours;
        [SerializeField] private IUnitInteract[] _interacts;
#pragma warning restore 0649

        private IList<ICommand> _commands;
        private Object _commandLock;
        private ICommand _currentCommand;
        private IBehaviour _currentBehaviour;

        // [SerializeField]
        public string guiName 
        {
            get
            {
                return _guiName;
            }
        }

        public IPlayer player 
        {
            get
            {
                return _player;
            }
            set
            {
                if(_player == null)
                    _player = value;
            }
        }

        public void GUILostFocus()
        {}

        public void GUIReceivedFocus()
        {}

        public void PutCommand(ICommand command, bool enfilerate)
        {
            lock (_commandLock)
            {
                if(_currentCommand == null)
                {
                    _currentCommand = command;
                            
                    if (command != null && !TryFindBehaviour(command, out _currentBehaviour))
                        Debug.Log("Não há um comportamento que executa o comando recebido " + command.GetType().Name);
                    else if (command == null)
                    {
                        _currentBehaviour = null;
                    }
                    else
                        _currentBehaviour.Setup(gameObject);
                }
                else
                {
                    if(enfilerate)
                    {
                        _commands.Add(command);
                    }
                    else
                    {
                        if(_currentBehaviour != null)
                        {
                            _currentBehaviour.Cancel(gameObject, _currentCommand);
                            _currentBehaviour.Reset();
                        }
                        _currentCommand = command;
                        
                        if (command != null && !TryFindBehaviour(command, out _currentBehaviour))
                            Debug.Log("Não há um comportamento que executa o comando recebido " + command.GetType().Name);
                        else if (command == null)
                        {
                            _currentBehaviour = null;
                        }
                        else
                            _currentBehaviour.Setup(gameObject);
                    }
                }
            }
            
        }

        public void PutInteract(GameObject target, bool enfilerate)
        {
            lock (_commandLock)
            {
                // ICommand command;

                // foreach (IUnitInteract i in interacts)
                // {
                //     if (i.Interact(gameObject, target, out comm))
                // }

                if(_currentCommand == null)
                {
                    if (!TryFindInteract(target, out _currentCommand, out _currentBehaviour))
                        Debug.Log("Não há uma interação adequada para que " + gameObject.name + " tenha um comportamento para " + target.name);
                }
                else
                {
                    if(enfilerate)
                    {
                        _commands.Add(new InteractCommand(target));
                    }
                    else
                    {
                        if(_currentBehaviour != null)
                        {
                            _currentBehaviour.Cancel(gameObject, _currentCommand);
                            _currentBehaviour.Reset();
                        }
                        
                        if (!TryFindInteract(target, out _currentCommand, out _currentBehaviour))
                            Debug.Log("Não há uma interação adequada para que " + gameObject.name + " tenha um comportamento para " + target.name);
                    }
                }
            }
        }

        private IBehaviour FindBehaviour(ICommand command)
        {
            foreach (IBehaviour b in _behaviours)
            {
                if (b.Condition(command))
                    return b;
            }
            return null;
        }
        private bool TryFindBehaviour(ICommand command, out IBehaviour behaviour)
        {
            foreach (IBehaviour b in _behaviours)
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
            foreach (IUnitInteract i in _interacts)
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
#pragma warning disable 0649
            [SerializeField] private Light selection;
            
            [SerializeField] private Color _normal;
            [SerializeField] private Color _selected;
            [SerializeField] private Color _hover;
#pragma warning restore 0649

            private bool selected;
            public void UnidDeselect()
            {
                if (!enabled)
                    return;
                selection.color = player.color * _normal;
                selected = false;
            }

            public void UnitSelect()
            {
                if (!enabled)
                    return;
                selection.color = player.color * _selected;
                selected = true;
            }
            public void UnitHover()
            {
                if (!enabled)
                    return;
                if (!selected)
                    selection.color = player.color * _hover;
                // else
                //     selection.color = player.color * _selected * _hover;
            }

            public void UnitNotHover()
            {
                if (!enabled)
                    return;
                if(selected)
                    selection.color = player.color * _selected;
                else
                    selection.color = player.color * _normal;
            }
        #endregion
        

        public void UpdateCommand()
        {
            if (_currentBehaviour != null)
            {
                lock (_commandLock)
                {
                    if (!_currentBehaviour.UpdateBehaviour(gameObject, _currentCommand))
                    {
                        _currentBehaviour.Reset();
                        _currentBehaviour = null;
                    }
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _commandLock = new Object();
            _commands = new List<ICommand>();
            if (player != null && selection != null)
            {
                UnidDeselect();
            }
        }

        // Update is called once per frame
        void Update()
        {
            UpdateCommand();
        }

        public void UnitStarter(UnitStarter.StarterEvent evt)
        {
            player = evt.currentPlayer;
            evt.gameManager.UnitNotify(gameObject, player);
        }

    }
}