using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Grid
{
    public class GridTest : MonoBehaviour
    {

        public GridMap map;
        public TilemapRender render;

        // Start is called before the first frame update
        void Start()
        {
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    map.Add(new Vector2Int(x + 5, (int) Math.Pow(y, 2)), "builds");
                }
                
            }

            render.Ocupado = map.GetLayer("builds");
            render.Repaint();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}