using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{

    UnitBehaviour select;
    MouseInput mouseInput;

    public Color color;


    private void Awake() {
        mouseInput = new MouseInput();
    }

    private void OnEnable() {
        mouseInput.Enable();
    }

    private void OnDisable() {
        mouseInput.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        mouseInput.Mouse.SelectClick.performed += _ => MouseClick();
        mouseInput.Mouse.CommandClick.performed += _ => MoveUnits();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void MouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput.Mouse.MousePosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null){
                if (hit.collider.GetComponent<UnitBehaviour>() != null)
                {
                    Debug.Log("Colisão detectada");
                    select = hit.collider.GetComponent<UnitBehaviour>();
                } else {
                    select = null;
                }
                
            } else {
                select = null;
            }
        }
    }

    private void MoveUnits()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput.Mouse.MousePosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null){
                // hit.normal
                GameObject surface = hit.collider.gameObject;
                if (surface.isStatic && select != null)
                {
                    NavMeshAgent agent = select.gameObject.GetComponent<NavMeshAgent>();
                    agent.destination = hit.point;
                }
                
            }
        }
    }

    private void DetectObject(){

    }
}
