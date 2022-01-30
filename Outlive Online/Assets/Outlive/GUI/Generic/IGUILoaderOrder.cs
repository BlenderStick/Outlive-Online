using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.GUI.Generic
{
    public interface IGUILoaderOrder
    {
        IEnumerable<IGUILoader> order(IEnumerable<IGUILoader> loaders);
    }
}