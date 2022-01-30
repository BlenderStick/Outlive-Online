using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.GUI.Generic;
using UnityEngine;

namespace Outlive.GUI
{
    [CreateAssetMenu(fileName = "new Order Loader", menuName = "Outlive/GUI/Order")]
    public class GUIOrderLoader : ScriptableObject, Generic.IGUILoaderOrder
    {
        [SerializeField] private Generic.GenericGUILoader[] _loaders;
        public IEnumerable<IGUILoader> order(IEnumerable<IGUILoader> loaders)
        {
            Dictionary<int, IGUILoader> reorderLoaders = new Dictionary<int, IGUILoader>();
            List<IGUILoader> nonOrdened = new List<IGUILoader>();
            
            foreach (var item in loaders)
            {
                if (reorderLoaders.ContainsValue(item) || nonOrdened.Contains(item))
                    continue;

                int index = Array.IndexOf(_loaders, item);
                if (index != -1)
                    reorderLoaders.Add(index, item);
                else
                    nonOrdened.Add(item);
            }

            for (int i = 0; i < _loaders.Length; i++)
            {
                IGUILoader iLoader;
                if (reorderLoaders.TryGetValue(i, out iLoader))
                    yield return iLoader;
            }
            foreach (var item in nonOrdened)
                yield return item;
        }
    }
}