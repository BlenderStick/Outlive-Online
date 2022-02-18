using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Grid
{
    public class GridMap : MonoBehaviour
    {
        [SerializeField] private string[] _layers;
        [SerializeField] private Vector2Int _size;

        private Dictionary<string, Dictionary<Vector2Int, int>> _grids;

        public string[] Layers 
        {
            get
            {
                string[] layers = new string[_grids.Count];
                _grids.Keys.CopyTo(layers, 0);
                return layers;
            }
        }

        private void Awake() 
        {
            _grids = new Dictionary<string, Dictionary<Vector2Int, int>>();

            foreach (var item in _layers)
                _grids.Add(item, new Dictionary<Vector2Int, int>());
        }

        public void Add(Vector2Int point, params string[] layers)
        {
            if (layers.Length == 0 || !Inside(point))
                return;

            foreach (var item in layers)
            {
                Dictionary<Vector2Int, int> set;
                if (_grids.TryGetValue(item, out set))
                {
                    if (set.ContainsKey(point))
                        set[point] += 1;
                    else
                        set.Add(point, 1);
                }
                    
            }
        }
        public void Remove(Vector2Int point, params string[] layers)
        {
            if (layers.Length == 0)
                return;

            foreach (var item in layers)
            {
                Dictionary<Vector2Int, int> set;
                if (_grids.TryGetValue(item, out set))
                {
                    if (set.ContainsKey(point))
                    {
                        int value = (set[point] -= 1);
                        if (value <= 0)
                            set.Remove(point);
                    }
                }
            }
        }

        public bool Contains(Vector2Int point)
        {
            foreach (var item in _grids.Values)
            {
                if (item.ContainsKey(point))
                    return true;
            }
            return false;
        }
        public bool Contains(Vector2Int point, params string[] layers)
        {
            foreach (var item in layers)
            {
                Dictionary<Vector2Int, int> set;
                if (_grids.TryGetValue(item, out set))
                    if (set.ContainsKey(point))
                        return true;
            }
            return false;
        }

        public bool Inside(Vector2Int point)
        {
            if (point.x < 0 || point.y < 0)
                return false;

            return point.x <= _size.x && point.y <= _size.y;
        }

        public HashSet<Vector2Int> GetAllLayers()
        {
            HashSet<Vector2Int> newSet = new HashSet<Vector2Int>();
            foreach (var item in _grids.Values)
            {
                newSet.UnionWith(item.Keys);
            }
            return newSet;
        }

        public HashSet<Vector2Int> GetLayers(params string[] layers)
        {
            HashSet<Vector2Int> newSet = new HashSet<Vector2Int>();

            if (layers.Length == 0)
                return newSet;
                

            foreach (var item in layers)
            {
                Dictionary<Vector2Int, int> set;
                if (_grids.TryGetValue(item, out set))
                    newSet.UnionWith(set.Keys);
            }

            return newSet;
        }

        public HashSet<Vector2Int> GetLayer(string layer)
        {
            return new HashSet<Vector2Int>(_grids[layer].Keys);
        }
        
    }
}