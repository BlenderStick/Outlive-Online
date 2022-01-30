using System.Collections;
using System.Collections.Generic;
using Outlive.GUI.Generic;
using UnityEngine;

namespace Outlive.GUI
{
    public class GUILoader : Generic.GenericGUILoader
    {
        [SerializeField] private GameObject _UIElement;
        public override void leave(IGUILoaderEvent evt)
        {
            throw new System.NotImplementedException();
        }

        public override void load(IGUILoaderEvent evt)
        {
            throw new System.NotImplementedException();
        }
    }
}