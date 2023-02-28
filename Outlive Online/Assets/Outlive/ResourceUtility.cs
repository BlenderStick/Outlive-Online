using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Grid;
using Outlive.Grid.Render.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Outlive
{

    //Class contains all default cost of prefabs
    ///<summary>O custo para construção de cada prefab</summary>
    internal class PrefabCost
    {
        ///<summary>O custo para construção de cada prefab</summary>
        public static int GetCost(string prefabName)
        {
            switch (prefabName)
            {
                case Outlive.PrefabsName.HM_CONSTRUCTOR:
                    return 250;
                case Outlive.PrefabsName.HM_FABRICA:
                    return 500;
                default:
                    return 0;
            }
        }
    }
    public static class PrefabsName
    {
        public const string HM_FABRICA = "hm_fabrica";
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
            _loader.Add(PrefabsName.HM_CONSTRUCTOR, () => Constructor_Main);
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

    public static class ResourcePath
    {
        public const string HumanPath = "Outlive/Human";
        
        #region Prefab Paths
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

    //class named PrefabInfo contain points around world center, cost to initialize, type name, and prefab
    public class PrefabInfo
    {
        public Vector2Int[] Points {get; private set;}
        public int Cost {get; private set;}
        public string Name {get; private set;}
        public GameObject Prefab {get; private set;}
        public PrefabInfo(Vector2Int[] points, int cost, string name, GameObject prefab)
        {
            Points = points;
            Cost = cost;
            Name = name;
            Prefab = prefab;
        }

        public IEnumerable<Vector2Int> GetPoints(Vector2Int center)
        {
            foreach (var point in Points)
                yield return point + center;
        }
    }

    ///<summary>Carrega os prefabs e separa pelo nome</summary>
    public class PrefabInfoLoader
    {
        private static Dictionary<string, PrefabInfo> _loader;
        private static void InstallPrefabs()
        {
            _loader = new Dictionary<string, PrefabInfo>();

            Vector2Int[] points = new Vector2Int[]{
                new Vector2Int(0,0),
                new Vector2Int(0,1),
                new Vector2Int(0,-1),
                new Vector2Int(1,0),
                new Vector2Int(-1,0),
            };

            _loader.Add(PrefabsName.HM_CONSTRUCTOR, 
                new PrefabInfo(
                    new Vector2Int[]{new Vector2Int(0,0)}, 
                    PrefabCost.GetCost(PrefabsName.HM_CONSTRUCTOR), 
                    PrefabsName.HM_CONSTRUCTOR, 
                    Resources.Load<GameObject>(ResourcePath.HM_FABRICA)));

            _loader.Add(PrefabsName.HM_FABRICA, 
                new PrefabInfo(
                    points, 
                    PrefabCost.GetCost(PrefabsName.HM_FABRICA), 
                    PrefabsName.HM_FABRICA, 
                    Resources.Load<GameObject>(ResourcePath.HM_FABRICA)));
        }

        ///<summary>Adiciona um novo prefab ao dicionário</summary>
        public static void AddPrefab(string name, PrefabInfo prefabInfo)
        {
            if (_loader == null)
                InstallPrefabs();
            _loader.Add(name, prefabInfo);
        }
        ///<summary>Retorna o PrefabInfo de um prefab</summary>
        public static PrefabInfo GetPrefab(string name)
        {
            if (_loader == null)
                InstallPrefabs();
            if (!_loader.ContainsKey(name))
                throw new ArgumentException($"Não há nenhum prefab chamado '{name}' na lista de prefabs");
            return _loader[name];
        }

    }
}