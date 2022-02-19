using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Outlive.Grid
{
    public class TilemapRender : MonoBehaviour
    {

        [SerializeField] private Tilemap _tilemap;

        public void Paint(Vector2Int point, MapTileType type)
        {
            _tilemap.SetTile(new Vector3Int(point.x, point.y, 0), GetTile(type));
        }

        public void Fill(RectInt rect, MapTileType type)
        {
            foreach (var item in rect.allPositionsWithin)
            {
                _tilemap.SetTile(new Vector3Int(item.x, item.y, 0), GetTile(type));
            }
        }

        public void Clear()
        {
            _tilemap.ClearAllTiles();
        }

        private static Tile GetTile(MapTileType type)
        {
            if (type == MapTileType.Obstacle)
                return Outlive.GridLoad.Obstaculo;
            if (type == MapTileType.Blocked)
                return Outlive.GridLoad.Ocupado;
            if (type == MapTileType.Free)
                return Outlive.GridLoad.Ocupavel;

            return Outlive.GridLoad.Vazio;
        }
    }

    ///<summary>Representa as cores que os Tiles assumirão</summary>
    public enum MapTileType
    {
        ///<summary>Tile branco</summary>
        Void,
        ///<summary>Tile amarelo</summary>
        Obstacle,
        ///<summary>Tile vermelho</summary>
        Blocked,
        ///<summary>Tile verde</summary>
        Free
    }
}