using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Grid.Render.Generic
{
    public interface ITileOption
    {
        ///<summary>Verifica se possui essa propriedade configurada</summary>
        bool HaveOption(string name);
        ///<summary>Pega o tile da camada mais importante para este TileOption</summary>
        MapTileType GetTile(params string[] layers);
    }
}