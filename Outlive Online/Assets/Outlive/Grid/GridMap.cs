using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Outlive.Grid
{
    public class GridMap : MonoBehaviour
    {
        [SerializeField] private string[] _layers;
        [SerializeField] private RectInt _bounds;

        [SerializeField] private UnityEvent<CallbackContext> _onTileChange;

        private Dictionary<string, Dictionary<Vector2Int, int>> _grids;
        private Dictionary<string, ReaderWriterLock> _gridLock;

        public string[] Layers 
        {
            get
            {
                string[] layers = new string[_grids.Count];
                _grids.Keys.CopyTo(layers, 0);
                return layers;
            }
        }

        public UnityEvent<CallbackContext> OnTileChange { get => _onTileChange; set => _onTileChange = value; }
        public RectInt Bounds { get => _bounds;}

        public class CallbackContext
        {
            public CallbackContext(GridMap gridMap, Vector2Int point, string layer, TileContextState state)
            {
                GridMap = gridMap;
                Point = point;
                Layer = layer;
                State = state;
            }

            public GridMap GridMap {get; private set;}
            public Vector2Int Point {get; private set;}
            public string Layer {get; private set;}
            ///<summary> O motivo do evento ser lançado (ganhar ou perder o Tile)</summary>
            public TileContextState State {get; private set;}
        }

        private void Awake() 
        {
            _grids = new Dictionary<string, Dictionary<Vector2Int, int>>(_layers.Length);
            _gridLock = new Dictionary<string, ReaderWriterLock>(_layers.Length);

            foreach (var item in _layers)
            {
                _grids.Add(item, new Dictionary<Vector2Int, int>());
                _gridLock.Add(item, new ReaderWriterLock());
            }

        }

        public void Add(Vector2Int point, params string[] layers)
        {
            if (layers.Length == 0 || !Inside(point))
                return;

            foreach (var item in layers)
            {
                ReaderWriterLock rwl;
                if (!_gridLock.TryGetValue(item, out rwl))
                    continue;

                bool isAlreadyAdded = false;

                rwl.AcquireWriterLock(5000);

                Dictionary<Vector2Int, int> set = _grids[item];
                if (set.ContainsKey(point))
                {
                    set[point] += 1;
                    isAlreadyAdded = true;
                }
                else
                    set.Add(point, 1);

                rwl.ReleaseWriterLock();

                //Lança do evento
                if (!isAlreadyAdded)
                    OnTileChange.Invoke(new CallbackContext(this, point, item, TileContextState.Gained));
            }
        }
        public void Remove(Vector2Int point, params string[] layers)
        {
            if (layers.Length == 0)
                return;

            foreach (var item in layers)
            {
                ReaderWriterLock rwl;
                if (!_gridLock.TryGetValue(item, out rwl))
                    continue;
                rwl.AcquireWriterLock(5000);
                Dictionary<Vector2Int, int> set = _grids[item];

                bool isAlreadyAdded = false;

                if (set.ContainsKey(point))
                {
                    int value = (set[point] -= 1);
                    if (value <= 0)
                    {
                        set.Remove(point);
                        isAlreadyAdded = true;
                    }
                }

                rwl.ReleaseWriterLock();

                //Lança o evento
                if (isAlreadyAdded)
                    OnTileChange.Invoke(new CallbackContext(this, point, item, TileContextState.Lost));
            }
        }

        public bool Contains(Vector2Int point)
        {
            
            foreach (var item in _grids)
            {
                ReaderWriterLock rwl;
                if (!_gridLock.TryGetValue(item.Key, out rwl))
                    continue;

                rwl.AcquireReaderLock(5000);

                try
                {
                    if (item.Value.ContainsKey(point))
                    {
                        return true;
                    }
                }
                finally
                {
                    rwl.ReleaseReaderLock();
                }
                
            }
            
            return false;
        }
        public bool Contains(Vector2Int point, params string[] layers)
        {
            foreach (var item in layers)
            {
                
                ReaderWriterLock rwl;
                if (!_gridLock.TryGetValue(item, out rwl))
                    continue;

                rwl.AcquireReaderLock(5000);

                try
                {
                   if (_grids[item].ContainsKey(point))
                        return true;
                }
                finally
                {
                    rwl.ReleaseReaderLock();
                }
            }

            return false;
        }

        public bool Inside(Vector2Int point)
        {
            return Bounds.Contains(point);
        }

        public int TileCount(Vector2Int point, string layer)
        {
            if (!_grids.ContainsKey(layer))
                throw new ArgumentException($"Não existe nenhuma layer com o nome '{layer}'.");
            ReaderWriterLock rwl = _gridLock[layer];

            rwl.AcquireReaderLock(5000);

            try
            {
                if (!_grids[layer].ContainsKey(point))
                    return 0;

                return _grids[layer][point];
            }
            finally
            {
                rwl.ReleaseReaderLock();
            }
        }


        ///<summary>
        /// Define o valor da contagem do tile <paramref name="point"/> da camada <paramref name="layer"/>.
        ///<para>
        ///Evite utilizar isso para realizar contagens personalizadas, use Add ou Remove para adicionar ou subtrair a contagem
        ///</para>
        ///</summary>
        public void SetTileCount(Vector2Int point, string layer, int count)
        {
            if (!_grids.ContainsKey(layer))
                throw new ArgumentException($"Não existe nenhuma layer com o nome '{layer}'.");
            if (count < 0)
                throw new ArgumentException("Não é possivel atribuir um valor menor do que 0 para o tile.");

            ReaderWriterLock rwl = _gridLock[layer];

            rwl.AcquireWriterLock(5000);

            bool isAlreadyAdded = _grids[layer].ContainsKey(point);

            try
            {

                if (count == 0)
                    _grids[layer].Remove(point);
                else
                    _grids[layer][point] = count;

            }
            finally
            {
                rwl.ReleaseWriterLock();
                if (isAlreadyAdded && count == 0)
                    OnTileChange.Invoke(new CallbackContext(this, point, layer, TileContextState.Lost));
                else if (count > 0)
                    OnTileChange.Invoke(new CallbackContext(this, point, layer, TileContextState.Gained));
                
            }
        }

        public HashSet<Vector2Int> GetAllLayers()
        {
            HashSet<Vector2Int> newSet = new HashSet<Vector2Int>();
            foreach (var item in _grids)
            {
                ReaderWriterLock rwl = _gridLock[item.Key];

                rwl.AcquireReaderLock(5000);
                newSet.UnionWith(item.Value.Keys);
                rwl.ReleaseReaderLock();
            }
            return newSet;
        }

        public IEnumerable<string> LayersThatHave(Vector2Int point)
        {
            foreach (var item in _grids)
            {
                _gridLock[item.Key].AcquireReaderLock(5000);
                try
                {
                    if (item.Value.ContainsKey(point))
                        yield return item.Key;
                }
                finally
                {
                    _gridLock[item.Key].ReleaseReaderLock();
                }
            }
        }

        public HashSet<Vector2Int> GetLayers(params string[] layers)
        {
            HashSet<Vector2Int> newSet = new HashSet<Vector2Int>();

            if (layers.Length == 0)
                return newSet;
                

            foreach (var item in layers)
            {
                
                ReaderWriterLock rwl;
                if (!_gridLock.TryGetValue(item, out rwl))
                    continue;
                rwl.AcquireReaderLock(5000);

                try
                {
                    newSet.UnionWith(_grids[item].Keys);
                }
                finally
                {
                    rwl.ReleaseReaderLock();
                }
                
            }

            return newSet;
        }

        public HashSet<Vector2Int> GetLayer(string layer)
        {
            try
            {
                if (!_gridLock.ContainsKey(layer))
                    throw new KeyNotFoundException($"Não existe uma layer chamada '{layer}'");
                _gridLock[layer].AcquireReaderLock(5000);
                return new HashSet<Vector2Int>(_grids[layer].Keys);
            }
            finally
            {
                if (_gridLock.ContainsKey(layer))
                    _gridLock[layer].ReleaseReaderLock();
            }
        }

        public IEnumerable<Vector2Int> GetLayerIterable(string layer)
        {
            if (!_grids.ContainsKey(layer))
                yield break;

            try
            {
                _gridLock[layer].AcquireReaderLock(5000);
                foreach (var item in _grids[layer])
                {
                    yield return item.Key;
                }
            }
            finally
            {
                _gridLock[layer].ReleaseReaderLock();
            }
        }
        
    }

    ///<summary> O motivo do evento ser lançado (ganhar ou perder o Tile)</summary>
    public enum TileContextState
    {
        Lost,
        Gained
    }
}