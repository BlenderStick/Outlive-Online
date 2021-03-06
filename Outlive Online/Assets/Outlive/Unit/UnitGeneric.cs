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
using Outlive.Controller;

namespace Outlive.Unit
{
    [AddComponentMenu("Outlive/Unit/Unit"), ExecuteInEditMode]
    public abstract class UnitGeneric : MonoBehaviour, ICommandableUnit, IGUIUnit, ISelectableUnit, IPlayerInjectable
    {

#pragma warning disable 0649
        [SerializeField] private IUnitInteract[] _interacts;
        [SerializeField] private PlayerSelect _player;
        [SerializeField] private PlayerInjector _injector;
        [SerializeField] private UnityEvent<IPlayer> _updatePlayerDependency;
        [SerializeField] private UnityEvent<Color> _onColorChange;
#pragma warning restore 0649
        private IPlayer _playerProperty;

        private List<ICommand> _commands;
        private object _commandLock;
        private ICommand _currentCommand;
        private IBehaviour _currentBehaviour;
        private bool _cancelCommand;

        public IPlayer player 
        {
            get
            {
                return _playerProperty;
            }
            set
            {
                if(_playerProperty == null)
                    _playerProperty = value;
            }
        }

        protected IBehaviour Behaviour => _currentBehaviour;

        public abstract string UnitName { get; }

        public virtual void PutCommand(ICommand command, bool enfilerate)
        {
            lock (_commandLock)
            {
                if (!enfilerate)
                {
                    if (_currentBehaviour != null)
                        _cancelCommand = true;
                    foreach (var item in _commands)
                        item.Skip();
                    _commands.Clear();
                }

                _commands.Add(command);
            }
            
        }

        protected abstract bool TryCreateBehaviour(ICommand command, out IBehaviour behaviour);
        
        public  abstract bool CanExecuteCommand(string commandName);

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

            lock (_commandLock)
            {
                if (_currentBehaviour == null || !_currentBehaviour.UpdateBehaviour(gameObject, _currentCommand, _cancelCommand))
                {
                    if (NextCommand())
                        _cancelCommand = false;
                }
                // Debug.Log(condition);
            }
        }

        private bool NextCommand()
        {
            if (_commands.Count == 0)
                return false;
            
            _currentCommand?.Skip();
            _currentCommand = _commands[0];
            _currentBehaviour?.Dispose();

            _commands.RemoveAt(0);
            if (TryCreateBehaviour(_currentCommand, out _currentBehaviour))
                _currentBehaviour.Setup(gameObject, _currentCommand);
            
                
            return true;
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
            _injector.UpdateManager(this);
        }

        // Update is called once per frame
        void Update()
        {
            UpdateCommand();
        }

        #region Injectable

        void IPlayerInjectable.OnInjectablePlayerListChange(IGameManager manager, string[] players)
        {
            _player.SetPlayerList(players);
        }

        void IPlayerInjectable.OnInjectablePlayerChange(IGameManager manager, string lastName, string currentName, Color lastColor, Color currentColor)
        {
            _player.UpdateName(lastName, currentName);

            if (_player.isPlayerUndefined)
            {
                _onColorChange.Invoke(Color.white);
                return;
            }

            if (_player.PlayerName == currentName)
                _onColorChange.Invoke(currentColor);

        }

        void IPlayerInjectable.OnGameManagerStart(IGameManager manager)
        {
            if (_player.isPlayerUndefined)
                return;
                
            _playerProperty = manager.GetPlayer(_player.PlayerName);
            _updatePlayerDependency.Invoke(_playerProperty);
        }

        void IPlayerInjectable.OnInjectorSet(PlayerInjector injector)
        {
            _injector = injector;
        }

        internal void ForceInjectorUpdate()
        {
            if (!_player.isPlayerUndefined)
                _injector.UpdateInjectable(this, _player.PlayerName);
            else
                _onColorChange.Invoke(Color.white);
        }


        #endregion
    }
}