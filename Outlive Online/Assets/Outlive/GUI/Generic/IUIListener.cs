using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.GUI.Generic
{
    public interface IUIListener
    {
        void onLoad(IGUILoaderEvent evt);
        void onLeave(IGUILoaderEvent evt);
    }
}