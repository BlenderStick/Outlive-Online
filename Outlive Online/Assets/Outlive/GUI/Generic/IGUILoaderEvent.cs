using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.GUI.Generic
{
    public interface IGUILoaderEvent
    {
        RectTransform root{get;}
        IGUILoader current{get;set;}

        GUIManager manager{get;}
    }
}