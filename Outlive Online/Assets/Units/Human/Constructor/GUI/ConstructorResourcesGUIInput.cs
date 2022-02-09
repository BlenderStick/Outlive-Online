using System.Collections;
using System.Collections.Generic;
using Outlive.GUI;
using Outlive.GUI.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConstructorResourcesGUIInput : MonoBehaviour, IUIListener
{

    private GUIManager _guiManager;
    [SerializeField] private GUILoader _constructorMain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Back(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
            
        _guiManager.guiLoader = _constructorMain;
    }

    public void onLoad(IGUILoaderEvent evt)
    {
        _guiManager = evt.manager;
    }

    public void onLeave(IGUILoaderEvent evt)
    {
    }
}
