using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Outlive
{
    public static class Units
    {
        public const string HM_CONSTRUCTOR = "hm_const";

    }

    public static class Commands
    {
        public const string MOVE = "mv";
    }

    public static class GUILoad
    {
        private static GameObject gui_constructor_main;
        private static GameObject gui_constructor_basic;
        private static GameObject gui_constructor_resources;

        private static Dictionary<string, Func<GameObject>> _loader;

        [RuntimeInitializeOnLoadMethod]
        private static void InstallGUIs()
        {
            _loader = new Dictionary<string, Func<GameObject>>();
            _loader.Add(Units.HM_CONSTRUCTOR, () => Constructor_Main);
        }

        public static GameObject GetGUI(string unitName) => _loader[unitName].Invoke();
        
        public static GameObject Constructor_Main
        {
            get
            {
                if (gui_constructor_main == null)
                    gui_constructor_main = Resources.Load<GameObject>(ResourcePath.HM_CONSTRUCTOR_GUI_MAIN);
                return gui_constructor_main;
            }
        }
        public static GameObject Constructor_Basic
        {
            get
            {
                if (gui_constructor_basic == null)
                    gui_constructor_basic = Resources.Load<GameObject>(ResourcePath.HM_CONSTRUCTOR_GUI_BASIC);
                return gui_constructor_basic;
            }
        }
        public static GameObject Constructor_Resources
        {
            get
            {
                if (gui_constructor_resources == null)
                    gui_constructor_resources = Resources.Load<GameObject>(ResourcePath.HM_CONSTRUCTOR_GUI_RES);
                return gui_constructor_resources;
            }
        }

    }

    public static class UnitsLoad
    {
        
        private static GameObject unit_constructor;

        public static GameObject Constructor
        {
            get
            {
                if (unit_constructor == null)
                    unit_constructor = Resources.Load<GameObject>(ResourcePath.HM_CONSTRUCTOR);
                return unit_constructor;
            }
        }
    }

    public static class ResourcePath
    {
        public const string HumanPath = "Outlive/Human";
        
        #region Unit
            public const string HM_CONSTRUCTOR = HumanPath + "/Units/Constructor";
        #endregion
        

        #region GUI
            public const string HM_CONSTRUCTOR_GUI_MAIN = HumanPath + "/GUI/ConstructorMainGUI";
            public const string HM_CONSTRUCTOR_GUI_BASIC = HumanPath + "/GUI/ConstructorBasicGUI";
            public const string HM_CONSTRUCTOR_GUI_RES = HumanPath + "/GUI/ConstructorResourcesGUI";
        #endregion
        
    }

    public static class GridLoad
    {
        ///<summary>Tile branco, não destrua</summary>
        public static Tile Vazio {get; private set;}
        ///<summary>Tile vermelho, não destrua</summary>
        public static Tile Ocupado {get; private set;}
        ///<summary>Tile amarelo, não destrua</summary>
        
        public static Tile Obstaculo {get; private set;}
        ///<summary>Tile verde, não destrua</summary>
        
        public static Tile Ocupavel {get; private set;}

        [RuntimeInitializeOnLoadMethod]
        private static void LoadResources()
        {
            Vazio = Resources.Load<Tile>("Grid/Tile_0");
            Ocupado = Resources.Load<Tile>("Grid/Tile_2");
            Obstaculo = Resources.Load<Tile>("Grid/Tile_3");
            Ocupavel = Resources.Load<Tile>("Grid/Tile_1");
        }
    }
}