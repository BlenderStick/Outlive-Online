using System;
using System.Net;
using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Outlive.Unit.Command;
using Outlive.Unit.Generic;
using Outlive.Human.Generic;
using Outlive.Human.Construcoes;
using Outlive.Controller;
using Outlive.GUI.Generic;
using Outlive.GUI;
using System.Threading.Tasks;

public class ConstructorMainGUIInput : MonoBehaviour, IUIListener
{

    [SerializeField] private GenericGUILoader _constructorBasicLoader;
    [SerializeField] private GenericGUILoader _constructorResourcesLoader;
    private Vector2 mousePosition;
    private bool isAttack;
    private bool isRepair;

    private PlayerController controller;
    private GUIManager manager;

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActionClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        if (isAttack)
        {
            Vector3 coord;
            if (RayCastInMap(mousePosition, out coord))
            {
                AttackCommand command = new AttackCommand(coord);
                foreach (GameObject b in controller.Selection.Selected)
                {
                    ICommandableUnit commandable;
                    if (b.TryGetComponent(out commandable))
                        commandable.PutCommand(command, false);
                }
            }



            isAttack = false;
            controller.EnableInputs(this);
        }
        if (isRepair)
        { 
            RaycastHit hit;
            // int layerMask = 1 << LayerMask.NameToLayer("Default");
            if (Physics.Raycast(controller.Camera.ScreenPointToRay(mousePosition), out hit))
            {
                Collider collider = hit.collider;
                IUnitConstructable uConst;

                if (collider.TryGetComponent(out uConst))
                {
                    foreach (GameObject item in controller.Selection.Selected)
                    {
                        ICommandableUnit commandable;
                        if (item.TryGetComponent(out commandable))
                            commandable.PutCommand(new BuildCommand(uConst.constructable), false);
                    }
                }
            }

            isRepair = false;
            controller.EnableInputs(this);
        }
    }
    public void CancelClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        if(isAttack)
        {
            controller.EnableInputs(this);
            isAttack = false;
        }
        if (isRepair)
        {
            controller.EnableInputs(this);
            isRepair = false;
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        isAttack = true;

        controller.DisableInputs(this);
        
    }

    public void Stand(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        foreach (GameObject item in controller.Selection.Selected)
        {
            ICommandableUnit commandable;
            if (item.TryGetComponent(out commandable))
                commandable.PutCommand(null, false);
        }
    }

    public void BasicConstructions(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        // new SetGUI(player, "constructorBasic");
        manager.guiLoader = _constructorBasicLoader;
    }
    public void ResourceConstructions(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        // new SetGUI(player, "constructorResources");
        manager.guiLoader = _constructorResourcesLoader;
    }

    public void RepairConstruction(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        controller.DisableInputs(this);
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
        // controller.EnableInputs(this);
        // if (player != null)
        //     player.defaultInput.enabled = true;
    }

    private bool RayCastInMap(Vector2 position, out Vector3 coordenates)
    {
        RaycastHit hit;
        if (Physics.Raycast(controller.Camera.ScreenPointToRay(position), out hit))
        {
            if (hit.collider.GetComponent<ICommandableUnit>() == null)
            {
                coordenates = hit.point;
                return true;
            }
        }
        // coordenates = controller.Camera.ScreenPointToRay(position).
        coordenates = Vector3.zero;
        return false;
    }

    public void onLoad(IGUILoaderEvent evt)
    {
        manager = evt.manager;
    }

    public void onLeave(IGUILoaderEvent evt)
    {
        // Task task = nullLoader();
        // task.Start();
        // await Task.Run(nullLoader);
    }
    // async Task nullLoader()
    // {
    //     await Task.Run(() => guiLoaderEvent = null);
    // }
}
