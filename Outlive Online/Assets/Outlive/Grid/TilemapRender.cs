using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Outlive.Grid
{
    public class TilemapRender : MonoBehaviour
    {

        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Vector2Int _bounds;


        public HashSet<Vector2Int> Ocupado { get; set; }
        public HashSet<Vector2Int> Obstaculo { get; set; }
        public HashSet<Vector2Int> Verde { get; set; }
        public Vector2Int Bounds { get => _bounds; set => _bounds = value; }

        public void Repaint()
        {
            if (_tilemap == null)
                return;

            for (int x = 0; x < _bounds.x; x++)
                for (int y = 0; y < _bounds.y; y++)
                    _tilemap.SetTile(new Vector3Int(x, y, 0), Outlive.GridLoad.Vazio);

            if (Ocupado != null)
                foreach (var item in Ocupado)
                    _tilemap.SetTile(new Vector3Int(item.x, item.y, 0), Outlive.GridLoad.Ocupado);

            if (Obstaculo != null)
                foreach (var item in Ocupado)
                    _tilemap.SetTile(new Vector3Int(item.x, item.y, 0), Outlive.GridLoad.Obstaculo);

            if (Verde != null)
                foreach (var item in Ocupado)
                    _tilemap.SetTile(new Vector3Int(item.x, item.y, 0), Outlive.GridLoad.Ocupavel);

        }
    }
}