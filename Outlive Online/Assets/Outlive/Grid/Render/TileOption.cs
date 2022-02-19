using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Grid.Render.Generic;
using UnityEngine;

namespace Outlive.Grid.Render
{
    [Serializable]
    public class TileOption: ITileOption
    {
        [SerializeField] private Option[] _options;

        public TileOption()
        {
        }
        public TileOption(Option[] options)
        {
            if (options == null)
                throw new ArgumentNullException();
            _options = options;
        }
        
        ///<summary>Verifica se possui essa propriedade configurada</summary>
        public bool HaveOption(string name)
        {
            if (_options == null)
                return false;

            return false;
        }

        public MapTileType GetTile(params string[] layers)
        {
            if (_options != null)
                foreach (var item in _options)
                    foreach (var name in layers)
                        if (item.LayerAfect == name)
                            return item.Tile;
            if (layers.Length == 0)
                throw new ArgumentException("Não é permitido uma lista vazia");
            throw new ArgumentException($"Não existe um TileOption.Option que atenda à layer '{layers}'.");
        }
        [Serializable]
        public class Option
        {
            [SerializeField] private string _layer;
            [SerializeField] private MapTileType _tile = MapTileType.Void;
            public Option(){}
            public Option(string layerAfect, MapTileType tile)
            {
                _layer = layerAfect;
                _tile = tile;
            }

            public string LayerAfect {get => _layer;}
            public MapTileType Tile {get => _tile;}
        }
        // public MapTileType 
    }
}