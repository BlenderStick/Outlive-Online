using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Grid.Render;
using UnityEngine;

namespace Outlive.Grid
{
    public class GridTest : MonoBehaviour
    {
        [SerializeField] private TileOption option;
        public GridMap map;
        public TilemapRender render;

        // Start is called before the first frame update
        void Start()
        {
            render.Fill(map.Bounds, MapTileType.Void);
        }

        public void OnTileChange(GridMap.CallbackContext ctx)
        {
            List<string> layers = new List<string>(3);
            foreach (var item in ctx.GridMap.LayersThatHave(ctx.Point))
                layers.Add(item);
            if (layers.Count > 0)
                render.Paint(ctx.Point, option.GetTile(layers.ToArray()));
            else
                render.Paint(ctx.Point, MapTileType.Void);
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}