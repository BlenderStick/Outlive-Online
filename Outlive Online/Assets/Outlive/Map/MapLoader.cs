using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Map
{
    [CreateAssetMenu(fileName = "New Map Loader", menuName = "Map/Loader")]
    public class MapLoader : ScriptableObject
    {
        [SerializeField] private List<MapSettings> _maps;
        [SerializeField] private List<Action<MapLoader>> _events;

        public IEnumerable<Generic.IMapSettings> maps {
            get => _maps;
        }

        public List<Action<MapLoader>> mapsChange => _events;

        public void upFolder(string folder)
        {

        }
        public bool downFolder()
        {
            return false;
        }

        private void fireMapChanged()
        {
            foreach (Action<MapLoader> evt in _events)
            {
                evt.Invoke(this);
            }
        }
    }
}