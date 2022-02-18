using Outlive.Controller;
using Outlive.GUI;
using Outlive.GUI.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConstructorBasicGUIInput : MonoBehaviour, IGUILoader
{
    private GUIManager guiManager;
    private PlayerController _controller;
    private Selection _selection;

    private Vector2 mousePosition;

    public void ActionClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;


    }

    public void CancelClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        
    }

    public void Back(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        guiManager.SetGUIPrefab(Outlive.GUILoad.Constructor_Main, _controller, _selection);
    }

    public void ConstructQuartel(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        
    }

    public void MousePosition(InputAction.CallbackContext ctx)
    {
        mousePosition = ctx.ReadValue<Vector2>();
    }

    void IGUILoader.Load(GUIManager.CallbackContext ctx)
    {
        guiManager = ctx.guiManager;
        _controller = ctx.controller;
        _selection = ctx.selection;
    }

    void IGUILoader.Update(GUIManager.CallbackContext ctx)
    {
    }

    void IGUILoader.Leave(GUIManager.CallbackContext ctx)
    {
        guiManager = null;
        Destroy(gameObject);
    }
}
