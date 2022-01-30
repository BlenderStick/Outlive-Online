using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Manager
{
    [Serializable]
    public class Player : Generic.IPlayer
    {
        
        [SerializeField] private string _displayName = "Novo Jogador";
        
        [SerializeField] private Color _color = Color.blue;

        private Generic.IGameManager _gameManager = null;
        
        [SerializeField] private GameManager _inspectorGameManager = null;
        public Player(Generic.IGameManager gameManager, string name, Color cor)
        {
            _gameManager = gameManager;
            _displayName = name;
            _color = cor;
        }
        public Color color 
        {
            get => _color;
        }

        public string displayName 
        {
            get => _displayName;
        }

        public void InteractUnits(Func<GameObject, bool> function)
        {
            foreach (GameObject obj in _gameManager.PlayerObjects(this))
            {
                if (!function.Invoke(obj))
                    return;
            }
        }

        public IEnumerable<GameObject> units => _gameManager.PlayerObjects(this);

        public void Awake() {
            if (_inspectorGameManager != null)
                _gameManager = _inspectorGameManager;
        }
    }
}