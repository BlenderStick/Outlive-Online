using System;
using System.Net;
using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Outlive.Unit.Command;
using Outlive.Unit.Generic;
using Outlive.Controller;
using Outlive.GUI.Generic;
using Outlive.GUI;
using System.Threading.Tasks;
using Outlive.Human.Generic;
using Outlive;
using Outlive.Grid;

public class ConstructorMainGUIInput : MonoBehaviour, IGUILoader
{
    [SerializeField] private LayerMask _mapLayer;

    private Vector2 mousePosition;
    private bool isAttack;
    private bool isRepair;

    private PlayerController _controller;
    private GUIManager manager;
    private Selection _selection;
    private GridMap _map;
    private PlayerCommander _playerCommander;

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
                foreach (GameObject b in _controller.Selection.Selected)
                {
                    ICommandableUnit commandable;
                    if (b.TryGetComponent(out commandable))
                        commandable.PutCommand(command, false);
                }
            }



            isAttack = false;
            _controller.EnableInputs(this);
        }
        if (isRepair)
        {
            if (_controller.Focus != null)
            {
                IConstructable constructable;
                if (_controller.Focus.TryGetComponent(out constructable))
                {
                    Repair(constructable, _controller.Focus.transform.position);
                }
            }
            isRepair = false;
            _controller.EnableInputs(this);
        }
    }
    public void CancelClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        if(isAttack)
        {
            _controller.EnableInputs(this);
            isAttack = false;
        }
        if (isRepair)
        {
            _controller.EnableInputs(this);
            isRepair = false;
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        isAttack = true;

        _controller.DisableInputs(this);
        
    }

    public void Stand(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        foreach (GameObject item in _controller.Selection.Selected)
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
        manager.SetGUIPrefab(Outlive.GUILoad.Constructor_Basic, _controller, _selection);
    }
    public void ResourceConstructions(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        // new SetGUI(player, "constructorResources");
        manager.SetGUIPrefab(Outlive.GUILoad.Constructor_Resources, _controller, _selection);
    }

    public void RepairConstruction(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        _controller.DisableInputs(this);
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

    private void Repair(IConstructable constructable, Vector3 center)
    {
        _playerCommander.BuildCommand(_selection, center, constructable, _controller.isMultselect);
    }

    private bool RayCastInMap(Vector2 position, out Vector3 coordenates)
    {
        RaycastHit hit;
        if (Physics.Raycast(_controller.Camera.ScreenPointToRay(position), out hit))
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

    void IGUILoader.Load(GUIManager.CallbackContext ctx)
    {
        _controller = ctx.Controller;
        manager = ctx.GuiManager;
        _selection = ctx.Selection;
        _map = ctx.GridMap;
        _playerCommander = ctx.PlayerCommander;
        // ctx.Commander
        // ctx.GridInteract.
    }

    void IGUILoader.Update(GUIManager.CallbackContext ctx)
    {
    }

    void IGUILoader.Leave(GUIManager.CallbackContext ctx)
    {
        _controller = null;
        manager = null;
        Destroy(gameObject);
    }
}
