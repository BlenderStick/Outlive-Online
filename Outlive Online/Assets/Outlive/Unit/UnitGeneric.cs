using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Unit.Interacts;
using Outlive.Unit.Command;
using Outlive.Unit.Generic;
using UnityEngine;
using Outlive.Manager.Generic;
using Outlive.GUI;
using Outlive.GUI.Generic;
using Outlive.Manager;
using UnityEngine.Events;

namespace Outlive.Unit
{
    [AddComponentMenu("Outlive/Unit/Unit"), ExecuteInEditMode]
    public class UnitGeneric : MonoBehaviour, ICommandableUnit, IGUIUnit, ISelectableUnit, IPlayerInjectable
    {

#pragma warning disable 0649
        [SerializeField] private GUILoader _guiLoader;
        [SerializeField] private BasicBehaviour[] _behaviours;
        [SerializeField] private IUnitInteract[] _interacts;
        [SerializeField, HideInInspector] private string[] _playersList;
        [SerializeField] private int _playerIndex;
        [SerializeField] private PlayerInjector _injector;
        [SerializeField] private UnityEvent<IPlayer> _updatePlayerDependency;
        [SerializeField] private UnityEvent<Color> _onColorChange;
#pragma warning restore 0649
        private IPlayer _player;

        private IList<ICommand> _commands;
        private object _commandLock;
        private ICommand _currentCommand;
        private IBehaviour _currentBehaviour;

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

        public IGUILoader guiLoader => _guiLoader;

        public virtual void PutCommand(ICommand command, bool enfilerate)
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

        public virtual void PutInteract(GameObject target, bool enfilerate)
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
            public void UnitDeselect()
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

        private void Awake()
        {
            if (!Application.IsPlaying(gameObject))
            {
                PlayerInjector injector = FindObjectOfType<PlayerInjector>();
                if (injector != null)
                    injector.AddInjectable(this);
            }
        }

        private void OnDestroy() 
        {
            if (Application.IsPlaying(gameObject))
                return;
                
            PlayerInjector injector = FindObjectOfType<PlayerInjector>();
            if (injector != null)
                injector.RemoveInjectable(this);
            
        }

        // Start is called before the first frame update
        void Start()
        {
            _commandLock = new object();
            _commands = new List<ICommand>();
            if (player != null && selection != null)
            {
                UnitDeselect();
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

        void IPlayerInjectable.OnInjectablePlayerListChange(IGameManager manager, string[] players)
        {
            _playersList = new string[players.Length + 1];
            _playersList[0] = "Undefined";
            Array.Copy(players, 0, _playersList, 1, players.Length);
        }

        void IPlayerInjectable.OnInjectablePlayerChange(IGameManager manager, string lastName, string currentName, Color lastColor, Color currentColor)
        {
            for (int i = 1; i < _playersList.Length; i++)
                if (_playersList[i] == lastName)
                {
                    _playersList[i] = currentName;
                    break;
                }

            if (_playerIndex < 1)
            {
                _onColorChange.Invoke(Color.white);
                return;
            }

            if (currentName == _playersList[_playerIndex])
            {
                _onColorChange.Invoke(currentColor);
            }
        }

        void IPlayerInjectable.OnGameManagerStart(IGameManager manager)
        {
            if (_playerIndex < 0)
                return;
                
            _player = manager.GetPlayer(_playersList[_playerIndex]);
            _updatePlayerDependency.Invoke(_player);
        }

        void IPlayerInjectable.OnInjectorSet(PlayerInjector injector)
        {
            _injector = injector;
        }

        internal void ForceInjectorUpdate(int newIndex)
        {
            if (newIndex > 0)
                _injector.UpdateInjectable(this, _playersList[newIndex]);
            else
                _onColorChange.Invoke(Color.white);
        }
    }
}