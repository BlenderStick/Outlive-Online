using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConstructorBasicGUIInput : MonoBehaviour
{
    
    private Player player;
    private Vector2 mousePosition;
    public Player Player { get => player; set => player = value; }

    public void ActionClick()
    {

    }

    public void CancelClick()
    {
        
    }

    public void Back()
    {
        player.setUnitGUI("constructorMain");
    }

    public void ConstructQuartel()
    {
        
    }

    public void MousePosition(InputAction.CallbackContext ctx)
    {
        mousePosition = ctx.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
