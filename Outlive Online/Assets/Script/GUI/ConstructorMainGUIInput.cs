using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Outlive.Unit.Command;
using Outlive.Unit.Generic;
using Outlive.Human.Generic;
using Outlive.Human.Construcoes;

public class ConstructorMainGUIInput : MonoBehaviour
{

    private Player player;
    private Vector2 mousePosition;
    private bool isAttack;
    private bool isRepair;

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
        if (isAttack)
        {
            ICommandableUnit[] units = player.GetSelectedUnits<ICommandableUnit>();
            Vector3 coord;
            if (player.RayCastInMap(mousePosition, out coord))
            {
                AttackCommand command = new AttackCommand(coord);
                foreach (ICommandableUnit b in units)
                {
                    b.PutCommand(command, false);
                }
            }



            isAttack = false;
            player.defaultInput.enabled = true;
        }
        if (isRepair)
        { 
            Collider collider;
            int layerMask = 1 << LayerMask.NameToLayer("Default");
            if (player.RayCast(mousePosition, out collider, layerMask))
            {
                IUnitConstructable uConst;

                if (collider.TryGetComponent<IUnitConstructable>(out uConst))
                {
                    ICommandableUnit[] units = player.GetSelectedUnits<ICommandableUnit>();
                    foreach (ICommandableUnit b in units)
                    {
                        b.PutCommand(new BuildCommand(uConst.constructable), false);
                    }
                }
            }

            isRepair = false;
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
        if (isRepair)
        {
            player.defaultInput.enabled = true;
            isRepair = false;
        }
    }

    public void Attack()
    {
        isAttack = true;
        player.defaultInput.enabled = false;
        
    }

    public void Stand()
    {
        ICommandableUnit[] units = player.GetSelectedUnits<ICommandableUnit>();
        foreach (ICommandableUnit b in units)
        {
            b.PutCommand(null, false);
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

    public void RepairConstruction()
    {
        player.defaultInput.enabled = false;
        isRepair = true;
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
