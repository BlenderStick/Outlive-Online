using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Human.Generic
{
    public interface IConstructable
    {
        bool AddBuildProgress(Vector2 builderPosition, int progress);
        bool NeedBuild {get;}
        int MissingProgress {get;}
        HashSet<Vector2Int> PositionsToBuild{get;}
    }
}