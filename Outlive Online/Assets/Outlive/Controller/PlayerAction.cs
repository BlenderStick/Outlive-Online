using System.Runtime.InteropServices.ComTypes;
using System.Net.Http.Headers;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Outlive.Manager.Generic;
using Outlive.Unit;
using Outlive.Unit.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Outlive.GUI;
using Outlive.GUI.Generic;
using static Outlive.Controller.PlayerController;
using UnityEngine.Events;

namespace Outlive.Controller
{
    ///<summary>
    ///Component responsável pela leitura das entradas do jogador como o mouse e o teclado.
    ///</summary>
    [AddComponentMenu("Outlive/Player/EventListener")]
    public class PlayerAction : MonoBehaviour
    {

        // [SerializeField] private GUIManager _guiManager;
        [SerializeField] private LayerMask _viewProjectLayer;
        [SerializeField] private UnityEvent<Rect> _onDragPerformed;
        [SerializeField] private UnityEvent _onDragEnd;
        [SerializeField] private UnityEvent<CallbackContext> _onSelectionChange;


        public UnityEvent<Rect> OnDragPerformed { get => _onDragPerformed;}
        public UnityEvent OnDragEnd { get => _onDragEnd;}
        public UnityEvent<CallbackContext> SelectionChange { get => _onSelectionChange;}


        private PlayerController _controller;
        private Vector3 _worldSelectBoxStarted;
        private bool _calculeSelectRect;
        private GameObject _focus;
        private Camera _camera;
        private Rect _dragRect;

        public void OnFocusChange(CallbackContextFocus ctx)
        {
            ISelectableUnit selectable;
            if (ctx.focus.TryGetComponent(out selectable))
            {
                if (ctx.state == SelectionState.Gained)
                {
                    _focus = ctx.focus;
                    selectable.UnitHover();
                }
                else
                {
                    _focus = null;
                    selectable.UnitNotHover();
                }
            }
        }
        public void OnSelect(PlayerController.CallbackContext ctx)
        {
            if (ctx.isPressed)
            {
                _dragRect = Rect.zero;

                RaycastHit hit;
                if (Physics.Raycast(ctx.controller.Camera.ScreenPointToRay(ctx.mousePosition), out hit, Mathf.Infinity, _viewProjectLayer))
                    _worldSelectBoxStarted = hit.point;
                _calculeSelectRect = true;
            }
            else
            {
                _calculeSelectRect = false;

                if (!ctx.isMultiselect)
                    ctx.controller.Selection.Clear();

                int count = 0;
                foreach (var item in ctx.player.units)
                {
                    if (_dragRect.Contains(ctx.controller.Camera.WorldToScreenPoint(item.transform.position)))
                    {
                        count ++;
                        ctx.controller.Selection.Add(item);
                    }
                }
                if (ctx.focus == null)
                {
                    _onSelectionChange.Invoke(new CallbackContext(ctx.controller, ctx.controller.Selection));
                    _onDragEnd.Invoke();
                    return;
                }
                
                if (count == 0)
                {
                    if (ctx.isMultiselect)
                    {
                        if (ctx.controller.Selection.Contain(ctx.focus))
                            ctx.controller.Selection.Remove(ctx.focus);
                        else
                            ctx.controller.Selection.Add(ctx.focus);
                    }
                    else
                        ctx.controller.Selection.Add(ctx.focus);
                }
                else
                    ctx.controller.Selection.Add(ctx.focus);
                    
                _onSelectionChange.Invoke(new CallbackContext(ctx.controller, ctx.controller.Selection));

                _onDragEnd.Invoke();
            }
        }
        public void OnCancelSelect(PlayerController.CallbackContext ctx)
        {
            _calculeSelectRect = false;


            _onDragEnd.Invoke();
            
        }
        
        public void OnInteract(PlayerController.CallbackContext ctx)
        {
        }

        public void OnMouseMove(InputAction.CallbackContext ctx)
        {
            if (!_calculeSelectRect && ctx.performed)
                return;

            _dragRect = createRect(_camera, ctx.ReadValue<Vector2>());

            foreach (var item in _controller.player.units)
            {
                if (item == _focus)
                    continue;

                ISelectableUnit selectable;
                if (item.TryGetComponent(out selectable))
                {

                    if (_dragRect.Contains(_camera.WorldToScreenPoint(item.transform.position)))
                        selectable.UnitHover();
                    else
                        selectable.UnitNotHover();
                }
            }

            _onDragPerformed.Invoke(_dragRect);
        }

        public void OnCameraChange(PlayerController controller)
        {
            _camera = controller.Camera;
            _controller = controller;
        }

        private Rect createRect(Camera camera, Vector2 mousePosition)
        {
            Vector2 rectPosition = Vector2.Min(camera.WorldToScreenPoint(_worldSelectBoxStarted), mousePosition);
            Vector2 rectSize = Vector2.Max(camera.WorldToScreenPoint(_worldSelectBoxStarted), mousePosition);
            
            return new Rect(rectPosition, rectSize - rectPosition);
        }

        public void OnSelectionChange(PlayerController.SelectionChangeCallback ctx)
        {
            ISelectableUnit selectable;
            if (ctx.current.TryGetComponent(out selectable))
            {
                if (ctx.state == SelectionState.Gained)
                    selectable.UnitSelect();
                else
                    selectable.UnitDeselect();
            }
        }

        public class CallbackContext
        {
            public CallbackContext(PlayerController controller, Selection selection)
            {
                this.controller = controller;
                this.selection = selection;
            }

            public PlayerController controller {get; private set;}
            public Selection selection {get; private set;}
        }
    }
}