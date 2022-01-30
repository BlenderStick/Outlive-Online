using System.Collections;
using System.Collections.Generic;
using Outlive.GUI.Generic;
using UnityEngine;

namespace Outlive.GUI.Generic
{
    public abstract class GenericGUILoader : ScriptableObject, Generic.IGUILoader
    {

         public abstract void leave(IGUILoaderEvent evt);


         public abstract void load(IGUILoaderEvent evt);
    }
}