using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConstructorMainGUIInput : MonoBehaviour
{

    private Player player;
    private Vector2 mousePosition;
    private bool isAttack;

    public Player Player { get => player; set => player = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActionClick()
    {
        if(isAttack)
        {
            ConstrutorBehaviour[] units = player.GetSelectedUnits<ConstrutorBehaviour>();
            Vector3 coord;
            if (player.RayCastInMap(mousePosition, out coord))
            {
                AttackCommand command = new AttackCommand(coord);
                foreach (ConstrutorBehaviour b in units)
                {
                    b.putCommand(command, false);
                }
            }



            isAttack = false;
            player.defaultInput.enabled = true;
        }
    }
    public void CancelClick()
    {
        if(isAttack)
        {
            player.defaultInput.enabled = true;
            isAttack = false;
        }
    }

    public void Attack()
    {
        isAttack = true;
        player.defaultInput.enabled = false;
        
    }

    public void Stand()
    {
        ConstrutorBehaviour[] units = player.GetSelectedUnits<ConstrutorBehaviour>();
        foreach (ConstrutorBehaviour b in units)
        {
            b.putCommand(null, false);
        }
    }

    public void BasicConstructions()
    {
        // new SetGUI(player, "constructorBasic");
        player.setUnitGUI("constructorBasic");
    }
    public void ResourceConstructions()
    {
        // new SetGUI(player, "constructorResources");
        player.setUnitGUI("constructorResources");
    }
    
    public void MousePosition(InputAction.CallbackContext ctx)
    {
        mousePosition = ctx.ReadValue<Vector2>();
        // Debug.Log(mousePosition);
    }

    private void OnDisable() 
    {
    }
    private void OnDestroy() 
    {
        Debug.Log("Destroy is called");
        // if (player != null)
        //     player.defaultInput.enabled = true;
    }

    // public void 
    private class SetGUI
    {
        public SetGUI(Player player, string guiName)
        {
            player.setUnitGUI(guiName);
        }
    }
}
