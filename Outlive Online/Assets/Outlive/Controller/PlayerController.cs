using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using Outlive.GUI;
using Outlive.GUI.Generic;
using Outlive.Manager.Generic;
using Outlive.Unit;
using Outlive.Unit.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Outlive.Controller.Generic;
using System;
using Outlive.Manager;

namespace Outlive.Controller
{
    [AddComponentMenu("Outlive/Player/Controller")]
    public class PlayerController : MonoBehaviour, IPlayerInjectable
    {

#pragma warning disable 0649
        [SerializeField] private PlayerInput _input;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private PlayerSelect _player;
        [SerializeField] private LayerMask _layerSelectable;
        [SerializeField] private UnityEvent<CallbackContextFocus> _onFocusChange;
        [SerializeField] private UnityEvent<CallbackContext> _onSelect;
        [SerializeField] private UnityEvent<CallbackContext> _onCancelSelect;
        [SerializeField] private UnityEvent<CallbackContext> _onInteract;
        [SerializeField] private UnityEvent<CallbackContext> _onMultiselectChange;
        [SerializeField] private UnityEvent<bool> _onEnableInputsChange;
        [SerializeField] private UnityEvent<Camera> _onCameraMove;
        [SerializeField] private UnityEvent<PlayerController> _onCameraChange;
        [SerializeField] private UnityEvent<SelectionChangeCallback> _onSelectionChange;
#pragma warning restore 0649

        public abstract class GenericCallback
        {
            public Vector2 mousePosition {get; internal set;}
            public GameObject focus {get; internal set;}
            public PlayerController controller {get; internal set;}
            public IPlayer player {get; internal set;}
        }

        public class CallbackContext : GenericCallback
        {

            internal CallbackContext(bool pressed, MouseActionType action, Vector2 position, bool multiSelect, GameObject focus, PlayerController playerController, IPlayer player)
            {
                isPressed = pressed;
                mouseAction = action;
                mousePosition = position;
                isMultiselect = multiSelect;
                this.focus = focus;
                controller = playerController;
                this.player = player;
            }
            public bool isPressed {get; private set;}
            public MouseActionType mouseAction {get; private set;}
            public bool isMultiselect {get; private set;}
        }

        public class CallbackContextFocus : GenericCallback
        {
            internal CallbackContextFocus(GameObject focus, SelectionState state, Vector2 position, PlayerController playerController, IPlayer player)
            {
                mousePosition = position;
                this.focus = focus;
                controller = playerController;
                this.player = player;
                this.state = state;
            }
            public SelectionState state {get; private set;}
        }
        
        public class SelectionChangeCallback
        {
            internal SelectionChangeCallback(GameObject current, SelectionState state, Selection selection, PlayerController controller)
            {
                this.current = current;
                this.state = state;
                this.selection = selection;
                this.controller = controller;
            }
            public GameObject current{get; private set;}
            public SelectionState state {get; private set;}
            public Selection selection {get; private set;}
            public PlayerController controller {get; private set;}
        }
        private Selection _selection;
        private IList<object> _inputDisableControl;
        public void DisableInputs(object obj)
        {
            _inputDisableControl.Add(obj);
            _input.enabled = false;
            _onEnableInputsChange.Invoke(false);
        }

        public void EnableInputs(object obj)
        {
            if (_inputDisableControl.Contains(obj))
                _inputDisableControl.Remove(obj);

            if (_inputDisableControl.Count == 0)
                _input.enabled = true;

            _onEnableInputsChange.Invoke(_input.enabled);
        }


        
        private Vector2 _mousePosition;
        private bool _enableInputs = true;
        private bool _isMultiselect;

        public bool enableInputs => _enableInputs;
        public IPlayer player{get; set;}
        public Selection Selection => _selection;
        public Camera Camera 
        {
            get => _mainCamera;
            set
            {
                _mainCamera = value;
                OnCameraChange.Invoke(this);
            }
        }
        private void Awake() {
            _selection = new Selection(listenSelectionChange);
        }
        private void Start() 
        {
            _inputDisableControl = new List<object>();
            _input = GetComponent<PlayerInput>();

            OnCameraChange.Invoke(this);
            _onEnableInputsChange.Invoke(true);
        }

        private void Update() 
        {
            UpdateGameObjectFocus();
        }

        public UnityEvent<CallbackContextFocus> OnFocusChange { get => _onFocusChange;}
        public UnityEvent<CallbackContext> OnSelect { get => _onSelect;}
        public UnityEvent<CallbackContext> OnCancelSelect { get => _onCancelSelect;}
        public UnityEvent<CallbackContext> OnInteract { get => _onInteract;}
        public UnityEvent<CallbackContext> OnMultiselectChange { get => _onMultiselectChange;}
        public UnityEvent<bool> OnEnableInputsChange { get => _onEnableInputsChange;}
        public UnityEvent<Camera> OnCameraMove { get => _onCameraMove;}
        public UnityEvent<PlayerController> OnCameraChange { get => _onCameraChange;}
        public UnityEvent<SelectionChangeCallback> OnSelectionChange { get => _onSelectionChange; set => _onSelectionChange = value; }

        private bool _selectCanceled;
        private bool _selectPerform;
        public void SelectDown(InputAction.CallbackContext context)
        {
            _selectPerform = context.performed;

            if (!enableInputs || !context.performed)
                return;

            if (context.performed)
                OnSelect.Invoke(new CallbackContext(true, MouseActionType.Select, _mousePosition, _isMultiselect, _currentFocused, this, player));
        }
        public void SelectUp(InputAction.CallbackContext context)
        {
            if (!enableInputs || !context.performed || _selectCanceled)
                return;

            if (context.performed)
                OnSelect.Invoke(new CallbackContext(false, MouseActionType.Select, _mousePosition, _isMultiselect, _currentFocused, this, player));
        }
        public void CancelSelect(InputAction.CallbackContext context)
        {
            _selectCanceled = context.performed;

            if (!context.performed)
                return;
                
            OnCancelSelect.Invoke(new CallbackContext(context.performed, MouseActionType.Undefined, _mousePosition, _isMultiselect, _currentFocused, this, player));
        }
        public void InteractClick(InputAction.CallbackContext context)
        {
            if (!enableInputs || !context.performed || _selectPerform)
                return;

            OnInteract.Invoke(new CallbackContext(context.performed, MouseActionType.Action, _mousePosition, _isMultiselect, _currentFocused, this, player));
        }
        public void MouseMove(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            _mousePosition = context.ReadValue<Vector2>();
        }
        public void Multiselect(InputAction.CallbackContext context)
        {
            _isMultiselect = context.performed;
            OnMultiselectChange.Invoke(new CallbackContext(false, MouseActionType.Undefined, _mousePosition, _isMultiselect, _currentFocused, this, player));
        }

        ///<summary> Chamado quando o jogador seleciona um grupo de unidades por meio de um botão, geralmente pelos números </summary>
        ///<param name="multiSelect"> <b>True</b> adiciona o grupo à seleção atual e <b>False</b> substitui a seleção atual pelo grupo </param>
        public void GroupSelect(int groupIndex, bool multiSelect){}
        ///<summary> Chamado quando o jogador define um grupo de unidades por meio de um botão, geralmente <b>Ctrl + número</b> </summary>
        public void GroupSet(int groupIndex){}
        public void GroupAdd(int groupIndex){}
        ///<summary> Envia a câmera do jogador até o grupo</summary>
        public void GroupShow(int groupIndex){}

        ///<summary>Collider da unidade selecionada atualmente</summary>
        private GameObject _currentFocused;
        private void UpdateGameObjectFocus()
        {
            RaycastHit hits;
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(_mousePosition), out hits, Mathf.Infinity, _layerSelectable))
            {
                if (hits.collider.gameObject != _currentFocused)
                {
                    fireFocusChange(_currentFocused, SelectionState.Lost);

                    _currentFocused = hits.collider.gameObject;
                    
                    fireFocusChange(_currentFocused, SelectionState.Gained);
                }
            }
            else
            {
                fireFocusChange(_currentFocused, SelectionState.Lost);

                _currentFocused = null;
            }
        }

        private void fireFocusChange(GameObject focus, SelectionState state)
        {
            if (focus != null)
                OnFocusChange.Invoke(new CallbackContextFocus(focus, state, _mousePosition, this, player));
        }

        private void listenSelectionChange(GameObject selected, Selection selection, bool gained)
        {
            _onSelectionChange.Invoke(
                new SelectionChangeCallback(
                    current:    selected, 
                    state:      gained? SelectionState.Gained : SelectionState.Lost, 
                    selection:  selection, 
                    controller: this));
        }

        void IPlayerInjectable.OnInjectablePlayerListChange(IGameManager manager, string[] players)
        {
            _player.SetPlayerList(players);
        }

        void IPlayerInjectable.OnInjectablePlayerChange(IGameManager manager, string lastName, string currentName, Color lastColor, Color currentColor)
        {
            _player.UpdateName(lastName, currentName);
        }

        void IPlayerInjectable.OnGameManagerStart(IGameManager manager)
        {
            player = manager.GetPlayer(_player.PlayerName);
        }

        void IPlayerInjectable.OnInjectorSet(PlayerInjector injector)
        {

        }
    }

    public enum MouseActionType
    {
        ///<summary> Ação do mouse não corresponde a nenhuma ação definida</summary>
        Undefined,
        ///<summary> Ação do mouse para selecionar <para> Botão esquerdo por padrão</para> </summary>
        Select,
        ///<summary> Ação do mouse para comandar <para> Botão direito por padrão </para> </summary>
        Action
    }

    public enum SelectionState
    {
        Gained,
        Lost
    }
}