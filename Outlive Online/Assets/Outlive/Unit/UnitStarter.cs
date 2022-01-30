using System.Collections;
using System.Collections.Generic;
using Outlive.Manager.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Outlive.Unit
{
    [AddComponentMenu("Outlive/Unit/Unit Starter")]
    public class UnitStarter : MonoBehaviour
    {

        public class StarterEvent
        {
            public StarterEvent(UnitStarter starter)
            {
                currentPlayer = starter.player;
                gameManager = starter.gameManager;
            }

            public IPlayer currentPlayer {get; private set;}
            public IGameManager gameManager {get; private set;}

        }
        #pragma warning disable 0649
        [SerializeField] private PlayerSelectionMode _mode;
        [SerializeField] private Object _playerReference;
        [SerializeField] private int _playerIndex;
        [SerializeField] private UnityEvent<StarterEvent> _onPlayerLoad;
        [SerializeField] private Object _gameManagerRef;
        #pragma warning restore 0649

        // Start is called before the first frame update
        void Start()
        {
            if (_gameManagerRef is IGameManager gameManager)
                SetPlayerWithGameManager(gameManager);
        }

        public void SetPlayerWithGameManager(IGameManager gameManager)
        {
            if (_mode == PlayerSelectionMode.INDEX)
                player = gameManager.GetPlayer(_playerIndex);
            else
            {
                if (_playerReference is IPlayer p)
                    player = p;
            }

            // if (player == null)
            //     return;
            _onPlayerLoad.Invoke(new StarterEvent(this));
        }

        public IPlayer player { get; private set; }
        public IGameManager gameManager {get => _gameManagerRef is IGameManager gameManager? gameManager : null;}

        public enum PlayerSelectionMode
        {
            INDEX,
            REFERENCE
        }
    }
}