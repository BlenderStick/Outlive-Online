using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Grid;
using Outlive.Grid.Render.Generic;
using UnityEngine;

namespace Outlive.Grid
{
    public class GridMapInteract : MonoBehaviour
    {

        [SerializeField] private TilemapRender _render;
        [SerializeField] private GridMap _map;

        private HashSet<Vector2Int> _interactPoints;

        private ITileOption _tileOption = Outlive.GridOption.DefaultTileOption;
        public HashSet<Vector2Int> InteractPoints
        {
            get => _interactPoints; 
            set 
            {
                HashSet<Vector2Int> changed  = OutliveUtilites.Difference(_interactPoints, value);
                _interactPoints = value;
                foreach (var item in changed)
                    Paint(item);
            }
        }

        public ITileOption TileOption 
        { 
            get => _tileOption; 
            set
            {
                _tileOption = value == null? Outlive.GridOption.DefaultTileOption : value; 
                RepaintAll();
            }
        }

        public bool TilemapVisibility
        {
            get => _render.Visibility;
            set => _render.Visibility = value;
        }
        // Start is called before the first frame update
        void Start()
        {
            RepaintAll();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnTileChange(GridMap.CallbackContext ctx)
        {
            Paint(ctx.Point);
        }

        public void Paint(Vector2Int point)
        {
            HashSet<string> layers = new HashSet<string>(_map.LayersThatHave(point));
            if (TileOption.HaveAllOptions(layers))
            {
                if (InteractPoints != null && InteractPoints.Contains(point))
                    _render.Paint(point, TileOption.GetTile(layers, true));
                else
                    _render.Paint(point, TileOption.GetTile(layers));
            }
            else
                _render.Paint(point, MapTileType.Blocked);
        }

        public void RepaintAll()
        {
            foreach (var item in _map.Bounds.allPositionsWithin)
            {
                Paint(item);
            }
        }

        public HashSet<string> LayersThatHave(HashSet<Vector2Int> points)
        {
            HashSet<string> layers = new HashSet<string>();

            foreach (var item in points)
            {
                layers.UnionWith(_map.LayersThatHave(item));
            }
            return layers;
        }
    }
}