using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Human.Generic
{
    public interface IConstructor
    {
        ///<summary>A construção que o construtor deverá tentar construir</summary>
        IConstructable Constructable {get; set;}
        bool IsBuilding {get;}
        Vector2Int PositionInGrid {get;}
    }
}