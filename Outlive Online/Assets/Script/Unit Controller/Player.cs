using System.Collections.Specialized;
using System.Threading;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Outlive;
using Outlive.Human.Construcoes;
using Outlive.Unit.Command;
using Outlive.Unit.Generic;

[Obsolete]
public class Player : MonoBehaviour
{
    // public MouseInput mouseInput;
    // public Input input;
    public PlayerManager manager;
    public PlayerInput defaultInput;
    private Outlive.Unit.Generic.ICommandableUnit Commandable;

    public Color color;

    private Vector3 mouseDragPosition;
    private bool drag;
    private Camera mainCamera;
    private List<GameObject> unitsInScene;
    private object unitsInSceleLock;
    private float dragTimeStarted;
    private float lastMouseMoved;
    private List<ICommandableUnit> units;
    private List<ISelectableUnit> unitsSelectables;
    private List<IGUIUnit> unitsGUI;
    private ISelectableUnit currentSelectable; 
    public Image selectionRectangle;
    public GameObject basicUnit;
    private Vector2 mousePosition;
    public GameObject GUIRoot;
    private string currentNameGUI = "";

    private bool isEnabledSelectWithClick = true;
    private bool isEnabledSelectBox = true;
    private bool isEnableActionWithClick = true;

    public bool isDisable;

    ///<summary>
    ///Determina se os comandos de click do mouse podem selecionar unidades.
    ///</summary>
    public bool EnableSelectWithClick { get => isEnabledSelectWithClick; set => isEnabledSelectWithClick = value; }
    ///<summary>
    ///Determina se os comandos de Drag and Drop do mouse podem criar a caixa de seleção de unidades.
    ///</summary>
    public bool EnableSelectBox { get => isEnabledSelectBox; set => isEnabledSelectBox = value; }
    ///<summary>
    ///Determina se os comandos de click do mouse podem dar ordens as unidades.
    ///</summary>
    public bool EnableActionWithClick { get => isEnableActionWithClick; set => isEnableActionWithClick = value; }

    private void Awake() {
        unitsInScene = new List<GameObject>();
        unitsInSceleLock = new object();
        units = new List<ICommandableUnit>();
        unitsSelectables = new List<ISelectableUnit>();
        unitsGUI = new List<IGUIUnit>();
    }

    // Start is called before the first frame update
    void Start()
    {

        if(!isDisable)
        {
            // Vector3Int[] vectors = MoveCommand.GenerateCircle(new Vector3(-2, 0, 5), new Vector3Int[0], 0, 10);
            // ICollection<Vector2Int> vects = OutliveUtilites.CalculatePointsAroundInGrid(new Vector2Int(-2, 5), 10);
            // ICollection<Vector2> vects2 = OutliveUtilites.CalculatePointsAround(new Vector2Int(-2, 5), 0.5f, 20, null, 0, 0.5f);
            // foreach (Vector3Int v in vectors)
            // {
            //     GameObject obj = Instantiate<GameObject>(basicUnit, v, Quaternion.Euler(0, 0, 0));
            //     UnitBehaviour b = obj.GetComponent<UnitBehaviour>();
            //     b.player = this;
            // }

            // foreach (Vector2Int v in vects)
            // {
            //     Vector3 pos = new Vector3(v.x, 0, v.y);
            //     GameObject obj = Instantiate<GameObject>(basicUnit, pos, Quaternion.Euler(0, 0, 0));
            //     UnitBehaviour b = obj.GetComponent<UnitBehaviour>();
            //     b.player = this;
            // }

            // foreach (Vector2 v in vects2)
            // {
            //     Vector3 pos = new Vector3(v.x, 0, v.y);
            //     GameObject obj = Instantiate<GameObject>(basicUnit, pos, Quaternion.Euler(0, 0, 0));
            //     UnitBehaviour b = obj.GetComponent<UnitBehaviour>();
            //     b.player = this;
            // }
        }
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDisable)
        {
            if(drag)
                drawSelectBox();  
            else
                hideSelectBox();
        }
        
    }
    public void SelectClick()
    {
        if (EnableSelectWithClick)
        {
            Collider collider;

            if(RayCast(mousePosition, out collider, LayerMask.GetMask("Default")))
            {
                ICommandableUnit commandable;
                if (collider.TryGetComponent<ICommandableUnit>(out commandable) && commandable.player == (object) this)
                {
                    units.Clear();
                    units.Add(commandable);
                    
                    ISelectableUnit selectable;
                    if (collider.TryGetComponent<ISelectableUnit>(out selectable))
                    {
                        DeselectAll();

                        unitsSelectables.Add(selectable);
                        selectable.UnitSelect();
                    }
                }

                IGUIUnit guiUnit;

                if (collider.TryGetComponent<IGUIUnit>(out guiUnit))
                {
                    unitsGUI.Clear();
                    unitsGUI.Add(guiUnit);
                }
            }
            updateUnitGUI();
        }
        
    }

    private void DeselectAll()
    {
        foreach (ISelectableUnit unit in unitsSelectables)
        {
            unit.UnitDeselect();
        }
        unitsSelectables.Clear();
    }
    private void SelectAll()
    {
        foreach (ISelectableUnit unit in unitsSelectables)
        {
            unit.UnitSelect();
        }
    }

    ///<summary>
    ///Lança um raio a partir de um ponto na vista do jogador e retorna true se acertar o mapa
    ///<para> 
    public bool RayCastInMap(Vector2 pointInScreen, out Vector3 surfacePoint)
    {
        RaycastHit hit;
        if(RayCast(pointInScreen, out hit, LayerMask.NameToLayer("Default")))
        {
            if (hit.collider.gameObject.isStatic)
            {
                surfacePoint = hit.point;
                return true;
            }
        }
        
        surfacePoint = mainCamera.ScreenToWorldPoint(pointInScreen);
        return false;
    }
    ///<summary>
    ///Lança um raio a partir de um ponto na vista do jogador e retorna true se acertar algum collider
    ///</summary>
    ///<param name="pointInScreen"> Posição na tela por onde o raio será lançado </param>
    ///<param name="collider"> Collider atingido </param>
    public bool RayCast(Vector2 pointInScreen, out Collider collider)
    {
        RaycastHit hit;
        if (RayCast(pointInScreen, out hit))
        {
            collider = hit.collider;
            return true;
        }
        collider = null;
        return false;
    }
    ///<summary>
    ///Lança um raio a partir de um ponto na vista do jogador e retorna true se acertar algum collider
    ///</summary>
    ///<param name="pointInScreen"> Posição na tela por onde o raio será lançado </param>
    ///<param name="collider"> Collider atingido </param>
    ///<param name="mask"> Mascara dos objetos que podem ser atingidos <param>
    public bool RayCast(Vector2 pointInScreen, out Collider collider, int layer)
    {
        RaycastHit hit;
        if (RayCast(pointInScreen, out hit, layer))
        {
            collider = hit.collider;
            return true;
        }
        collider = null;
        return false;
    }
    ///<summary>
    ///Lança um raio a partir de um ponto na vista do jogador e retorna true se acertar algum objeto
    ///</summary>
    ///<param name="pointInScreen"> Posição na tela por onde o raio será lançado </param>
    ///<param name="raycastHit"> Informações do raio </param>
    public bool RayCast(Vector2 pointInScreen, out RaycastHit raycastHit)
    {
        Ray ray = mainCamera.ScreenPointToRay(pointInScreen);
        return Physics.Raycast(ray, out raycastHit);
    }
    ///<summary>
    ///Lança um raio a partir de um ponto na vista do jogador e retorna true se acertar algum objeto
    ///</summary>
    ///<param name="pointInScreen"> Posição na tela por onde o raio será lançado </param>
    ///<param name="raycastHit"> Informações do raio </param>
    public bool RayCast(Vector2 pointInScreen, out RaycastHit raycastHit, int layer)
    {
        Ray ray = mainCamera.ScreenPointToRay(pointInScreen);
        // Physics.Raycast(ray, 100f, layer, )
        return Physics.Raycast(ray, out raycastHit, Mathf.Infinity, layer);
    }

    public void ActionClick()
    {
        if (EnableActionWithClick)
        {
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    GameObject surface = hit.collider.gameObject;
                    if (surface.isStatic && units.Count > 0)
                    {
                        Vector3Int target = Vector3Int.FloorToInt(
                            new Vector3(
                                x: hit.point.x + 0.5f,
                                y: hit.point.y,
                                z: hit.point.z + 0.5f));
                        GridManager gridManager = GetComponent<GridManager>();
                        System.Collections.Generic.IReadOnlyCollection<Vector3Int> grids = gridManager.GetGridPoints();
                        // Vector3Int[] targetVectors = MoveCommand.points(units, grids.ToArray(), grids.Count, target);
                        Vector3[] targetVectors = OutliveUtilites.From2DTo3DCoordinates(OutliveUtilites.CalculatePointsAround(new Vector2(hit.point.x, hit.point.z), 0.5f, units.Count, gridManager.Get2DMask(), grids.Count, 0f));
                        int count = 0;
                        foreach(ICommandableUnit unit in units)
                        {
                            unit.PutCommand(new MoveCommand(targetVectors[count]), false);
                            count ++;
                        }
                    } 
                }
            }
        }
        
    }

    public void MouseDrag(InputAction.CallbackContext context)
    {
        if (EnableSelectBox)
            if(context.performed)
                EventMouseDrag();
            else if(context.canceled)
                EventMouseDrop();
    }

    private void EventMouseDrag() 
    {
        dragTimeStarted = Time.realtimeSinceStartup;
        Vector2 mousePosition = this.mousePosition;
        mouseDragPosition = mainCamera.ScreenToViewportPoint(mousePosition);
        // Debug.Log("Drag " + mouseDragPosition);
        drag = true;
    }

    private void EventMouseDrop(){
        Vector2 mousePosition = this.mousePosition;
        // if (Time.realtimeSinceStartup - dragTimeStarted > 0.5){
        if(lastMouseMoved > dragTimeStarted)
        {
            Rect r = getSelectBox();
            lock (unitsInSceleLock)
            {
                units.Clear();
                unitsGUI.Clear();
                unitsSelectables.Clear();
                foreach (GameObject unit in unitsInScene)
                {
                    ICommandableUnit commandable;
                    if(unit.TryGetComponent<ICommandableUnit>(out commandable)) {
                        if(commandable.player == (object) this && r.Contains(mainCamera.WorldToScreenPoint(unit.transform.position))){
                            units.Add(commandable);
                            
                            // unit.GetComponent<PlayerInput>().actionEvents

                                    
                            IGUIUnit guiUnit;

                            if(unit.TryGetComponent<IGUIUnit>(out guiUnit))
                            {
                                unitsGUI.Add(guiUnit);
                            }

                            ISelectableUnit selectable;

                            if (unit.TryGetComponent<ISelectableUnit>(out selectable))
                            {
                                unitsSelectables.Add(selectable);
                                selectable.UnitSelect();
                            }
                        }
                    }

                }
                updateUnitGUI();
            }
            
        }
        
        drag = false;
    }

    private void updateUnitGUI(){
        List<string> names = new List<string>();
        foreach (IGUIUnit u in unitsGUI)
        {
            string n = "";
            if(!names.Contains(n))
                names.Add(n);
        }
        // if(GUIRoot != null)
        // {
            if (names.Count > 0)
                setUnitGUI(names[0]);
            else
                setUnitGUI("");
            // UnitGUI.InvokeUninstall(currentNameGUI, GUIRoot, this);

            // if(names.Count > 0)
            //     currentNameGUI = names[0];
            // else
            //     currentNameGUI = "";

            // UnitGUI.InvokeInstall(currentNameGUI, GUIRoot, this);
        // }
    }

    public void setUnitGUI(string name)
    {
        // lock (this)
        // {
            //  Debug.Log("Call's");
            if(GUIRoot != null)
            {
                string newName = name == null? "": name;


                UnitGUI.InvokeUninstall(currentNameGUI, GUIRoot, this);
                
                currentNameGUI = newName;
                
                UnitGUI.InvokeInstall(newName, GUIRoot, this);
            }
        
        // }
    }

    public void SetMousePosition(InputAction.CallbackContext context)
    {
        lastMouseMoved = Time.realtimeSinceStartup;
        mousePosition = context.ReadValue<Vector2>();
        SelectableUnitCheck();
    }

    private void SelectableUnitCheck()
    {
        Collider collider;
        if (RayCast(mousePosition, out collider))
        {
            ISelectableUnit selectable;
            if (collider.TryGetComponent<ISelectableUnit>(out selectable))
            {
                if (selectable != currentSelectable)
                    if(currentSelectable != null)
                        currentSelectable.UnitNotHover();
                selectable.UnitHover();
                currentSelectable = selectable;
            }
            else
            {
                if(currentSelectable != null)
                    currentSelectable.UnitNotHover();
            }
        }
    }
    public Vector2 GetMousePosition()
    {
        return mousePosition;
    }

    private void drawSelectBox()
    {
        selectionRectangle.enabled = true;
        RectTransform r = selectionRectangle.GetComponent<RectTransform>();

        Rect rect = getSelectBox();
        r.transform.position = rect.position;
        r.sizeDelta = rect.size;
    }
    private void hideSelectBox(){
        selectionRectangle.enabled = false;
    }

    /// <summary>
    /// Retorna um retângulo (Rect) que descreve a caixa de seleção de unidades do jogador
    /// </summary>
    private Rect getSelectBox()
    {
        Vector2 mousePosition = this.mousePosition;
        Vector2 lastMousePosition = mainCamera.ViewportToScreenPoint(mouseDragPosition);
        Vector2 p1 = new Vector2 (Mathf.Min(mousePosition.x, lastMousePosition.x), Mathf.Min(mousePosition.y, lastMousePosition.y));
        Vector2 p2 = new Vector2 (Mathf.Max(mousePosition.x, lastMousePosition.x), Mathf.Max(mousePosition.y, lastMousePosition.y));

        return new Rect(p1, p2 - p1);
    }

    private void StopSelectedUnits(){
        foreach (UnitBehaviour unit in units)
        {
            unit.PutCommand(null, false);
        }
    }
    public T[] GetSelectedUnits<T>() where T : ICommandableUnit
    {
        T[] tUnits = new T[units.Count];
        int index = 0;
        foreach (ICommandableUnit i in units)
        {
            if (i is T)
            {
                tUnits[index] = (T) i;
                index ++;
            }
        }

        T[] newTUnits = new T[index];
        System.Array.Copy(tUnits, newTUnits, index);
        return newTUnits;
    }

    public ICommandableUnit[] GetSelectedUnits(){
        return units.ToArray();
    }
    public GameObject[] GetUnitsInScene(){
        lock (unitsInSceleLock)
        {
            return unitsInScene.ToArray();
        }
    }

    public static void InstallPlayer(Player player, GameObject obj)
    {
        lock (player.unitsInSceleLock)
        {
            player.unitsInScene.Add(obj);
        }
    }
}