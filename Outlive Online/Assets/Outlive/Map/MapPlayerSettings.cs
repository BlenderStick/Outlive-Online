using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Outlive.Manager.Generic;
using System;

namespace Outlive.Map
{
    [Serializable]
    public class MapPlayerSettings : IPlayer
    { 
        [SerializeField] private Color _color = Color.black;
        [SerializeField] private string _displayName = "";
        public Color color => _color;

        public string displayName => _displayName;

        public IEnumerable<GameObject> units
        {
            get
            {
                yield break;
            }
        }

        public void Awake(){}
    }
}