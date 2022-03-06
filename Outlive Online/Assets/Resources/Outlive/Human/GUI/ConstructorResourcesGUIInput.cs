using System.Collections;
using System.Collections.Generic;
using Outlive.Controller;
using Outlive.GUI;
using Outlive.GUI.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConstructorResourcesGUIInput : MonoBehaviour, IGUILoader
{

    private GUIManager _guiManager;
    private PlayerController _controller;
    private Selection _selection;

    public void Back(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        _guiManager.SetGUIPrefab(Outlive.GUILoad.Constructor_Main, _controller, _selection);
            
    }

    void IGUILoader.Load(GUIManager.CallbackContext ctx)
    {
        _guiManager = ctx.GuiManager;
        _controller = ctx.Controller;
        _selection = ctx.Selection;
    }

    void IGUILoader.Update(GUIManager.CallbackContext ctx)
    {
    }

    void IGUILoader.Leave(GUIManager.CallbackContext ctx)
    {
        _guiManager = null;
        Destroy(gameObject);
    }
}
