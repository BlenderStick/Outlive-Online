using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Grid;
using Outlive.Grid.Render.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Outlive
{
    public static class UnitCreation
    {
        public static bool CanCreate(Vector2Int position, HashSet<Vector2Int> voidPoints)
        {
            return true;
        }
    }
    public static class Units
    {
        public const string HM_CONSTRUCTOR = "hm_const";

    }

    public static class Commands
    {
        public const string MOVE = "mv";
        public const string BUILD = "bd";
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
        private static GameObject build_quartel;

        public static GameObject Constructor
        {
            get
            {
                if (unit_constructor == null)
                    unit_constructor = Resources.Load<GameObject>(ResourcePath.HM_CONSTRUCTOR);
                return unit_constructor;
            }
        }
        public static GameObject Quartel
        {
            get
            {
                if (build_quartel == null)
                    build_quartel = Resources.Load<GameObject>(ResourcePath.HM_FABRICA);
                return build_quartel;
            }
        }
    }

    public static class ResourcePath
    {
        public const string HumanPath = "Outlive/Human";
        
        #region Unit
            public const string HM_CONSTRUCTOR = HumanPath + "/Units/Constructor";
            public const string HM_FABRICA = HumanPath + "/Builds/Fabrica";
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

    public static class GridOption
    {
        private class DefaultOption : ITileOption
        {

            public MapTileType GetTile(HashSet<string> layers, bool interacting = false)
            {
                if (layers == null || layers.Count == 0)
                    return interacting? MapTileType.Free: MapTileType.Void;

                
                if (layers.Contains("obstacles") || layers.Contains("builds") || layers.Contains("jazidas") || layers.Contains("enemys"))
                    return interacting? MapTileType.Blocked: MapTileType.Obstacle;
                

                throw new ArgumentException($"Não há nenhuma layer em '{layers}' que corresponda a um item da lista de opções");
            }

            public bool HaveAllOptions(IEnumerable<string> names)
            {
                foreach (var item in names)
                    if (!HaveOption(item))
                        return false;
                
                return true;
            }

            public bool HaveOption(string name)
            {
                return name == "builds" || name == "jazidas" || name == "enemys" || name == "obstacles" || name == "" || name == null;
            }
        }

        private class JazidaOption : ITileOption
        {

            public MapTileType GetTile(HashSet<string> layers, bool interacting = false)
            {
                if (layers == null || layers.Count == 0)
                    return interacting? MapTileType.Blocked: MapTileType.Void;

                if (layers.Contains("jazidas"))
                    return MapTileType.Free;

                if (layers.Contains("obstacles"))
                    return interacting? MapTileType.Blocked: MapTileType.Obstacle;

                if (layers.Contains("builds") || layers.Contains("enemys"))
                    return MapTileType.Blocked;



                throw new ArgumentException($"Não há nenhuma layer em '{layers}' que corresponda a um item da lista de opções");
            }

            public bool HaveAllOptions(IEnumerable<string> names)
            {
                foreach (var item in names)
                    if (!HaveOption(item))
                        return false;
                
                return true;
            }

            public bool HaveOption(string name)
            {
                return name == "builds" || name == "jazidas" || name == "enemys" || name == "obstacles" || name == "" || name == null;
            }
        }

        private static ITileOption _defaultTileOption;
        private static ITileOption _defaultJazidaTileOption;
        public static ITileOption DefaultTileOption
        {
            get
            {
                if (_defaultTileOption == null)
                    _defaultTileOption = new DefaultOption();
                return _defaultTileOption;
            }
        }
        public static ITileOption DefaultJazidaTileOption
        {
            get
            {
                if (_defaultJazidaTileOption == null)
                    _defaultJazidaTileOption = new JazidaOption();
                return _defaultJazidaTileOption;
            }
        }
    }
}