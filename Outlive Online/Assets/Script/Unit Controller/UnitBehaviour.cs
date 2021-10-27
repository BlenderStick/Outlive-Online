using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitBehaviour : MonoBehaviour
{

    public Player player;
    public Tilemap map;
    MouseInput mouseInput;

    private UnitBehaviour(){}
    public UnitBehaviour(Player player)
    {
        this.player = player;
    }

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
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", player.color);
        // mouseInput.Mouse.MouseClick.performed += _ => MouseClick();
    }

    private void MouseClick(){
        Vector2 mousePosition = mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int gridPosition  = map.WorldToCell(mousePosition);
        if (map.HasTile(gridPosition))
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void putCommand(PlayerCommand command, bool enfilerate){

    }

    void putInteract(){
        putCommand(new MoveCommand(0, 0), false);
    }
}
