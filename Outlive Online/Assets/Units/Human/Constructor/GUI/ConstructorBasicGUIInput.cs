using Outlive.GUI;
using Outlive.GUI.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConstructorBasicGUIInput : MonoBehaviour, IUIListener
{
    [SerializeField] private GUILoader _constructorMain;
    private GUIManager guiManager;
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
        
        guiManager.guiLoader = _constructorMain;
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

    private void Awake() 
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onLoad(IGUILoaderEvent evt)
    {
        guiManager = evt.manager;
    }

    public void onLeave(IGUILoaderEvent evt)
    {
    }
}
