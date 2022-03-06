using Outlive.Controller;
using Outlive.Grid;
using Outlive.GUI;
using Outlive.GUI.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConstructorBasicGUIInput : MonoBehaviour, IGUILoader
{
    private GUIManager guiManager;
    private PlayerController _controller;
    private Selection _selection;
    private GridMapInteract _gridInteract;

    private Vector2 mousePosition;
    private Vector2Int _gridPosition;

    private bool Build = false;
    private bool Quartel = false;
    [SerializeField] private LayerMask _chao;

    private void Awake() 
    {
        mousePosition = Mouse.current.position.ReadValue();
    }

    public void UpdateGridPosition(Vector2 mousePosition, bool force = false)
    {
        RaycastHit hit;
        if (Physics.Raycast(_controller.Camera.ScreenPointToRay(mousePosition), out hit, Mathf.Infinity, _chao))
        {
            Vector2Int current = Outlive.OutliveUtilites.PointToGrid(Outlive.OutliveUtilites.From3DTo2DCoordinates(hit.point));
            if (current == _gridPosition && !force)
                return;

            _gridPosition = current;

            _gridInteract.InteractPoints = Outlive.OutliveUtilites.CalculePointsAroundGrid(current, 13, null);
        } 
    }

    public void ActionClick(InputAction.CallbackContext context)
    {
        if (!context.performed || !Build)
            return;

        Build = false;
        _gridInteract.InteractPoints = null;
        _gridInteract.TileOption = null;
        _gridInteract.TilemapVisibility = false;
    }
    public void ActionRelease(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        _controller.EnableInputs(this);
    }

    public void CancelClick(InputAction.CallbackContext context)
    {
        if (!context.performed || !Build)
            return;

        _controller.EnableInputs(this);
        Build = false;
        _gridInteract.InteractPoints = null;
        _gridInteract.TileOption = null;
        _gridInteract.TilemapVisibility = false;
    }

    public void Back(InputAction.CallbackContext context)
    {
        if (!context.performed || Build)
            return;

        guiManager.SetGUIPrefab(Outlive.GUILoad.Constructor_Main, _controller, _selection);
    }

    public void ConstructQuartel(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        Build = true;
        Quartel = true;

        _controller.DisableInputs(this);
        UpdateGridPosition(mousePosition, true);
        // _gridInteract.TileOption = Outlive.GridOption.DefaultJazidaTileOption;
        _gridInteract.TilemapVisibility = true;
        
    }

    public void MousePosition(InputAction.CallbackContext ctx)
    {
        mousePosition = ctx.ReadValue<Vector2>();
        if (!Build)
            return;

        UpdateGridPosition(mousePosition);

    }

    void IGUILoader.Load(GUIManager.CallbackContext ctx)
    {
        guiManager = ctx.GuiManager;
        _controller = ctx.Controller;
        _selection = ctx.Selection;
        _gridInteract = ctx.GridInteract;
    }

    void IGUILoader.Update(GUIManager.CallbackContext ctx)
    {
    }

    void IGUILoader.Leave(GUIManager.CallbackContext ctx)
    {
        guiManager = null;
        _controller = null;
        _selection = null;
        _gridInteract = null;
        Destroy(gameObject);
    }
}
