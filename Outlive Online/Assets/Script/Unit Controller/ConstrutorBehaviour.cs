using System.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Outlive.Manager.Generic;
using System;

[Obsolete]
public class ConstrutorBehaviour : GenericFireUnit
{

    protected override string getGUIName()
    {
        return "constructorMain";
    }

    public ConstrutorBehaviour(IPlayer player) : base(player)
    {
        
    }
    

    // Start is called before the first frame update
    // void Start()
    // {
    //     // gui
    //     base.Start();
    // }

    // Update is called once per frame
    // void Update()
    // {
        
    // }





    //Criação de Interface
    private static GameObject ConstructorMainGUI;
    private static GameObject ConstructorBasicGUI;
    private static GameObject ConstructorResourcesGUI;

    [RuntimeInitializeOnLoadMethod]
    public static void registerGUI()
    {
        UnitGUI.PutMethodsToCreateGUI("constructorMain", ConstructGUIMain, ConstructGUIMain);
        UnitGUI.PutMethodsToCreateGUI("constructorBasic", ConstructGUIBasic, ConstructGUIBasic);
        UnitGUI.PutMethodsToCreateGUI("constructorResources", ConstructGUIResources, ConstructGUIResources);
        // ConstructorMainGUI = Resources.Load<GameObject>("Assets/GUI/Constructor GUIs/ConstructorBasicGUI.prefab");
    }
    public static void ConstructGUIMain(UnitGUI.GUIEvent evt)
    {
        if (evt.Install)
        {
            if (ConstructorMainGUI == null)
            {
                GameObject refObj = (GameObject) Resources.Load("GUI/ConstructorGUIs/ConstructorMainGUI");
                
                // EditorGUI
                ConstructorMainGUI = Instantiate<GameObject>(refObj, Vector3.zero, Quaternion.Euler(0, 0, 0), evt.Obj.transform);
                // ConstructorMainGUI.GetComponent<ConstructorMainGUIInput>().Player = evt.Player;
            }
            ConstructorMainGUI.SetActive(true);
        } 
        else 
        {
            ConstructorMainGUI.SetActive(false);
        }
    }
    public static void ConstructGUIBasic(UnitGUI.GUIEvent evt)
    {
        if (evt.Install)
        {
            if (ConstructorBasicGUI == null)
            {
                GameObject refObj = (GameObject) Resources.Load("GUI/ConstructorGUIs/ConstructorBasicGUI");
                
                // EditorGUI
                ConstructorBasicGUI = Instantiate<GameObject>(refObj, Vector3.zero, Quaternion.Euler(0, 0, 0), evt.Obj.transform);
                // ConstructorBasicGUI.GetComponent<ConstructorBasicGUIInput>().Player = evt.Player;
            }
            ConstructorBasicGUI.SetActive(true);
        } 
        else 
        {
            ConstructorBasicGUI.SetActive(false);
            // ConstructorBasicGUI.
            // ConstructorBasicGUI = null;
        }
    }
    public static void ConstructGUIResources(UnitGUI.GUIEvent evt)
    {
        if (evt.Install)
        {
            if (ConstructorResourcesGUI == null)
            {
                GameObject refObj = (GameObject) Resources.Load("GUI/ConstructorGUIs/ConstructorResourcesGUI");
                
                // EditorGUI
                ConstructorResourcesGUI = Instantiate<GameObject>(refObj, Vector3.zero, Quaternion.Euler(0, 0, 0), evt.Obj.transform);
                // ConstructorResourcesGUI.GetComponent<ConstructorResourcesGUIInput>().Player = evt.Player;
            }
            ConstructorResourcesGUI.SetActive(true);
        } 
        else 
        {
            ConstructorResourcesGUI.SetActive(false);
        }
    }
}
