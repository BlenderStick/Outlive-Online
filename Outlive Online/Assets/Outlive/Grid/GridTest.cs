using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Grid.Render;
using UnityEngine;

namespace Outlive.Grid
{
    public class GridTest : MonoBehaviour
    {
        public GridMap map;

        // Start is called before the first frame update
        void Start()
        {
            Vector2Int[] jazidas = new Vector2Int[]
            {
            new Vector2Int(0, 5),
            new Vector2Int(-5, 5),
            new Vector2Int(3, 2)
            };
            foreach (var item in Outlive.OutliveUtilites.CalculePointsAroundGrid(new Vector2Int(3, 4), 5, null))
            {
                map.Add(item, "jazidas");
            }
            foreach (var item in new RectInt(-2, 0, 5, 3).allPositionsWithin)
            {
                map.Add(item + jazidas[1], "obstacles");
            }
            // list.
        }

        private void PutCoords(string layer, Vector2Int offset, IEnumerable<Vector2Int> points)
        {
            foreach (var item in points)
                map.Add(item + offset, layer);
            
        }

        // Update is called once per frame
        void Update()
        {
        }

        private HashSet<Vector2Int> jazidaPoints = 
            new HashSet<Vector2Int>(
                new Vector2Int[]
                {
                    new Vector2Int(-1, 0),
                    new Vector2Int(+1, 0),
                    new Vector2Int(0, 0),
                    new Vector2Int(0, -1),
                    new Vector2Int(0, +1),
                }
                );
    }
}