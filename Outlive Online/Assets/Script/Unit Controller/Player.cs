using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private MouseInput mouseInput;
    public PlayerManager manager;

    public Color color;

    private Vector3 mouseDragPosition;
    private bool drag;
    private Camera mainCamera;
    private UnitBehaviour[] unitsInScene;
    private float dragTimeStarted;
    private float lastMouseMoved;
    private List<UnitBehaviour> units;
    public Image selectionRectangle;
    public GameObject basicUnit;

    
    public static int Nothing = 0;
    public static int Attack = 1;
    private int stateCommand;
    public bool isDisable;

    private void Awake() 
    {
        if(!isDisable)
            mouseInput = new MouseInput();
    }

    private void OnEnable() 
    {
        if(!isDisable)
            mouseInput.Enable();
    }

    private void OnDisable() 
    {
        if(!isDisable)
            mouseInput.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!isDisable)
        {
            mouseInput.Mouse.SelectClick.performed += _ => MouseClick();
            mouseInput.Mouse.CommandClick.performed += _ => MoveUnits();
            mouseInput.Mouse.MouseDrag.performed += _ => MouseDrag();
            mouseInput.Mouse.MouseDrag.canceled += _ => MouseDrop();
            mouseInput.Mouse.KeyAttack.performed += _ => SetStateCommand(Attack);
            mouseInput.Mouse.KeyStop.performed += _ => StopSelectedUnits();

            mouseInput.Mouse.MousePosition.performed += _ => 
                {
                    // isMouseMoving = true; 
                    lastMouseMoved = Time.realtimeSinceStartup;
                };

            mouseInput.Mouse.MousePosition.canceled += _ => 
                {
                    // isMouseMoving = false;
                };
            

            Vector3[] vectors = MoveCommand.GenerateCircle(new Vector3(-2, 0, 5), new Vector3[0], 0, 10);
            // int index = unitsInScene.Length;
            // UnitBehaviour[] newUnitsInScenne = new UnitBehaviour[index + 10];
            // System.Array.Copy(unitsInScene, newUnitsInScenne, index);
            foreach (Vector3 v in vectors)
            {
                GameObject obj = Instantiate<GameObject>(basicUnit, v, Quaternion.Euler(0, 0, 0));
                UnitBehaviour b = obj.GetComponent<UnitBehaviour>();
                b.player = this;
                // newUnitsInScenne[index] = b;
                // index ++;
            }
            // unitsInScene = newUnitsInScenne;
        }
        unitsInScene = GameObject.FindObjectsOfType<UnitBehaviour>();
        units = new List<UnitBehaviour>();
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
    private void MouseClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(mouseInput.Mouse.MousePosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                if (stateCommand == Nothing)
                {
                    if (hit.collider.GetComponent<UnitBehaviour>() != null)
                    {
                        units.Clear();
                        UnitBehaviour unitBehaviour = hit.collider.GetComponent<UnitBehaviour>();
                        if (unitBehaviour.player == this)
                            units.Add(unitBehaviour);
                    }
                    else 
                    {
                        units.Clear();
                    }
                    
                }
                else if (stateCommand == Attack)
                {
                    GameObject surface = hit.collider.gameObject;
                    if (surface.isStatic && units.Count > 0)
                    {
                        foreach(UnitBehaviour unit in units)
                        {
                            unit.putCommand(new AttackCommand(hit.point), false);
                        }
                        
                    }
                    SetStateCommand(Nothing);
                }
            }
            else 
            {
                units.Clear();
            }
        }
    }

    private void MoveUnits()
    {
        SetStateCommand(Nothing);
        Ray ray = mainCamera.ScreenPointToRay(mouseInput.Mouse.MousePosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                // hit.normal
                GameObject surface = hit.collider.gameObject;
                if (surface.isStatic && units.Count > 0)
                {
                    Vector3Int target = Vector3Int.FloorToInt(
                        new Vector3(
                            x:hit.point.x + 0.5f,
                            y: hit.point.y,
                            z: hit.point.z + 0.5f));
                    Debug.Log("target is: [" + target.x + ", " + target.z + "];");
                    string text = "Points is: {\n";
                    Vector3[] targetVectors = MoveCommand.points(units, null, 0, target);
                    foreach (Vector3 v in targetVectors)
                    {
                        text += "    [" + v.x + ", " + v.z + "],\n";
                    }
                    text += "    };";
                    Debug.Log(text);
                    int count = 0;
                    foreach(UnitBehaviour unit in units)
                    {
                        unit.putCommand(new MoveCommand(targetVectors[count]), false);
                        count ++;
                    }
                    
                }
                
            }
        }
    }

    private void MouseDrag()
    {
        dragTimeStarted = Time.realtimeSinceStartup;
        Vector2 mousePosition = mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
        mouseDragPosition = mainCamera.ScreenToViewportPoint(mousePosition);
        // Debug.Log("Drag " + mouseDragPosition);
        drag = true;
    }

    private void MouseDrop(){
        Vector2 mousePosition = mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
        // if (Time.realtimeSinceStartup - dragTimeStarted > 0.5){
        if(lastMouseMoved > dragTimeStarted)
        {
            units.Clear();
            Rect r = getSelectBox();
            foreach (UnitBehaviour unit in unitsInScene)
            {
                if(unit.player == this && r.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
                    units.Add(unit);
            }
        }
        drag = false;
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
        Vector2 mousePosition = mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
        Vector2 lastMousePosition = mainCamera.ViewportToScreenPoint(mouseDragPosition);
        Vector2 p1 = new Vector2 (Mathf.Min(mousePosition.x, lastMousePosition.x), Mathf.Min(mousePosition.y, lastMousePosition.y));
        Vector2 p2 = new Vector2 (Mathf.Max(mousePosition.x, lastMousePosition.x), Mathf.Max(mousePosition.y, lastMousePosition.y));

        return new Rect(p1, p2 - p1);
    }
    ///<summary>
    ///Define o tipo de comando que será executado pelas unidades selecionadas, como atacar, parar, construir.
    ///
    ///<paramref name="command"/>Tipo de comando, pode ser Nothing, Attack.
    ///</summary>
    private void SetStateCommand(int command){
        this.stateCommand = command;
    }

    private void StopSelectedUnits(){
        foreach (UnitBehaviour unit in units)
        {
            unit.putCommand(null, false);
        }
    }

    public UnitBehaviour[] GetUnits(){
        return units.ToArray();
    }
    public UnitBehaviour[] GetUnitsInScene(){
        return unitsInScene;
    }
}