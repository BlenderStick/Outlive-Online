using System.Net.Http.Headers;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Outlive.Manager.Generic;
using Outlive.Unit;
using Outlive.Unit.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Outlive.Controller
{
    ///<summary>
    ///Component responsável pela leitura das entradas do jogador como o mouse e o teclado.
    ///</summary>
    [AddComponentMenu("Outlive/Player/EventListener")]
    public class PlayerEventListener : MonoBehaviour
    {
        private Generic.IPlayerController _playerController;
#pragma warning disable 0649
        [SerializeField, HideInInspector] private Object _playerControllerRef;
        [SerializeField] protected RectTransform _selectBox;
        [SerializeField] private double _dragTimePerforme = 0.5d;
#pragma warning restore 0649
        private Vector2 _mousePosition;
        private Vector2 _mouseDragStart;
        private bool _isMultiselect;
        private IList<GameObject> _selectedUnits;
        // private ReaderWriterLock _selectedLock;


        private bool isDragging = false;
        private bool postDragging = false;
        private bool _enableInputs = true;

        public bool enableInputs => _enableInputs;
        public IPlayer player{get; set;}
        
        private void Awake() {
            _playerController = (Generic.IPlayerController) _playerControllerRef;
            _selectedUnits = new List<GameObject>();
        }
        private void Start() 
        {
            _selectBox.gameObject.SetActive(false);

        }

        private void Update() {
            if (isDragging)
            {
                Rect area = dragArea;
                _selectBox.position = area.position;
                _selectBox.sizeDelta = area.size;
                DrawSelectBoxHover(area);
                postDragging = true;
            }
            else if(postDragging)
            {
                NotHoverUnit();
                postDragging = false;
            }
        }

        public Rect dragArea
        {
            get
            {
                Vector2 coord1 = _mousePosition; //Não precisamos converter pois já é pré-convertido
                Vector2 coord2 = Camera.main.ViewportToScreenPoint(_mouseDragStart); //Convertemos para coordenadas de tela
                return Rect.MinMaxRect(
                    Mathf.Min(coord1.x, coord2.x), 
                    Mathf.Min(coord1.y, coord2.y),
                    Mathf.Max(coord1.x, coord2.x), 
                    Mathf.Max(coord1.y, coord2.y));
            }
        }


        public void ActionClick(InputAction.CallbackContext context)
        {
            if (!enableInputs || !context.performed)
                return;

            if (currentCommandable != null)
            {
                if (currentCommandable.GetComponent<ICommandableUnit>().player == player)
                {
                    if (_isMultiselect)
                    {
                        if (_selectedUnits.Contains(currentCommandable.gameObject))
                        {
                            _selectedUnits.Remove(currentCommandable.gameObject);
                            DeselectUnits(currentCommandable.gameObject);
                            MouseHover(true);
                        }
                        else
                        {
                            _selectedUnits.Add(currentCommandable.gameObject);
                        }
                    }
                    else
                    {
                        DeselectUnits();
                        _selectedUnits.Clear();
                        _selectedUnits.Add(currentCommandable.gameObject);
                    }
                    FireUnitSelect();
                }
                    
            }
            else
            {
                if (!_isMultiselect)
                {
                    DeselectUnits();
                    _selectedUnits.Clear();
                }
            }
        }
        public void CommandClick(InputAction.CallbackContext context){}
        public void MouseMove(InputAction.CallbackContext context){_mousePosition = context.ReadValue<Vector2>(); MouseHover();}
        public void MouseDrag(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!enableInputs)
                    return;

                _mouseDragStart = Camera.main.ScreenToViewportPoint(_mousePosition);
                _selectBox.gameObject.SetActive(true);
                isDragging = true;
            }
            if (context.canceled)
            {
                isDragging = false;
                _selectBox.gameObject.SetActive(false);
                if (context.time - context.startTime < _dragTimePerforme && dragArea.width * dragArea.height < Mathf.Pow(10, 2))
                    return;

                if (!_isMultiselect)
                {
                    DeselectUnits();
                    _selectedUnits.Clear();
                }
                SelectArea(dragArea);
                if (currentCommandable != null)
                    _selectedUnits.Add(currentCommandable.gameObject);
                FireUnitSelect();
            }
        }
        public void Multiselect(InputAction.CallbackContext context)
        {
            _isMultiselect = context.performed;
        }
        ///<summary> Chamado quando o jogador seleciona um grupo de unidades por meio de um botão, geralmente pelos números </summary>
        ///<param name="multiSelect"> <b>True</b> adiciona o grupo à seleção atual e <b>False</b> substitui a seleção atual pelo grupo </param>
        public void GroupSelect(int groupIndex, bool multiSelect){}
        ///<summary> Chamado quando o jogador define um grupo de unidades por meio de um botão, geralmente <b>Ctrl + número</b> </summary>
        public void GroupSet(int groupIndex){}
        public void GroupAdd(int groupIndex){}
        ///<summary> Envia a câmera do jogador até o grupo</summary>
        public void GroupShow(int groupIndex){}


        private void SelectArea(Rect area)
        {
            foreach (var item in player.units)
            {
                Vector2 viewPosition = Camera.main.WorldToScreenPoint(item.transform.position);
                if (area.Contains(viewPosition))
                    _selectedUnits.Add(item);
            }
        }

        ///<summary>Collider da unidade selecionada atualmente</summary>
        private Collider currentSelectable;
        private Collider currentCommandable;
        private void MouseHover(bool forceHover = false)
        {
            Collider target;
            if (_playerController.RayCast(_mousePosition, out target))
            {
                if (target != currentCommandable)
                {
                    if (target.gameObject.GetComponent<ICommandableUnit>() != null)
                        currentCommandable = target;
                    else
                        currentCommandable = null;
                }

                ISelectableUnit selectable;
                if (!target.gameObject.TryGetComponent<ISelectableUnit>(out selectable))
                {
                    if (currentSelectable != null && currentSelectable.gameObject.TryGetComponent<ISelectableUnit>(out selectable))
                        selectable.UnitNotHover();
                    currentSelectable = null;
                    return;
                }

                if (!forceHover && target == currentSelectable)
                    return;
                
                currentSelectable = target;

                selectable.UnitHover();

                // if (currentHover != null && target != currentHover)
                // {
                //     ISelectableUnit selectable2;
                //     if (currentHover.gameObject.TryGetComponent<ISelectableUnit>(out selectable2))
                //         {selectable2.UnitNotHover();Debug.Log("Not Hover");}
                // }

            }
            // else if (currentHover != null)
            // {
            //     ISelectableUnit selectable;
            //     if (currentHover.gameObject.TryGetComponent<ISelectableUnit>(out selectable))
            //         {selectable.UnitNotHover();Debug.Log("Old Not Hover");}
            //     currentHover = null;
                
            // }
        }

        private void DrawSelectBoxHover(Rect area)
        {
            foreach (var item in player.units)
            {
                if (currentSelectable != null && item == currentSelectable.gameObject)//Se o objeto cujo mouse está em cima for o item, pula o processo.
                    continue;

                Vector2 viewPosition = Camera.main.WorldToScreenPoint(item.transform.position);
                ISelectableUnit selectable;
                if (item.TryGetComponent<ISelectableUnit>(out selectable))
                {
                    if (area.Contains(viewPosition))
                        selectable.UnitHover();
                    else
                        selectable.UnitNotHover();
                }
            }
        }

        private void NotHoverUnit()
        {
            foreach (var item in player.units)
            {
                if (currentSelectable != null && item == currentSelectable.gameObject)
                    continue;

                ISelectableUnit selectable;
                if (item.TryGetComponent<ISelectableUnit>(out selectable))
                    selectable.UnitNotHover();
            }
        }

        private void DeselectUnits(IEnumerable<GameObject> units)
        {
            foreach (var item in units)
            {
                ISelectableUnit selectable;
                if (item.TryGetComponent<ISelectableUnit>(out selectable))
                    selectable.UnidDeselect();
            }
        }
        private void DeselectUnits(params GameObject[] units) => DeselectUnits((IEnumerable<GameObject>) units);

        private void DeselectUnits() => DeselectUnits(_selectedUnits);

        private void FireUnitSelect()
        {
            foreach (var item in _selectedUnits)
            {
                ISelectableUnit selectable;
                if (item.TryGetComponent<ISelectableUnit>(out selectable))
                    selectable.UnitSelect();
            }
        }

        public void OnUnitStarter(UnitStarter.StarterEvent evt)
        {
            player = evt.currentPlayer;
        }
    }
}