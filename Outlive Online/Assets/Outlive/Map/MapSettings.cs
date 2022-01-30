using System.Collections;
using System.Collections.Generic;
using Outlive.Manager.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Outlive.Map
{
    [CreateAssetMenu(fileName = "newMapSettings", menuName = "Map/MapSettings")]
    public class MapSettings : ScriptableObject, Generic.IMapSettings
    {

        public class TempPlayer : IPlayer
        {

            public TempPlayer (string name, Color cor)
            {
                displayName = name;
                color = cor;
            }
            public Color color {get; private set;}

            public string displayName {get; private set;}

            public IEnumerable<GameObject> units 
            {
                get
                {
                    yield break;
                }
            }

            public void Awake(){}
        }

        [SerializeField] protected Sprite _spriteIcon;
        [SerializeField] protected Sprite _spriteExtendedIcon;
        [SerializeField] protected string _mapName;

        [SerializeField] protected string _sceneMap;

        [SerializeField] List<MapPlayerSettings> _players;
        protected string[] _playerNames;
        public Sprite sprite { get => _spriteIcon;}

        public string mapName {get => _mapName;}

        public IEnumerable<IPlayer> playables => _playerNames == null ? _players : createdPlayables;

        private IEnumerable<IPlayer> createdPlayables
        {
            get
            {
                int count = 0;
                foreach (var item in _players)
                {
                    yield return new TempPlayer(_playerNames[count++], item.color);
                }
            }
        }

        public IEnumerable<bool> changeColor 
        {
            get
            {
                for (int i = 0; i < _players.Count; i++)
                {
                    yield return false;
                }
            }
        }

        public IEnumerable<bool> changeName
        {
            get
            {
                for (int i = 0; i < _players.Count; i++)
                {
                    yield return true;
                }
            }
        }


        public bool SetColor(int playerIndex, Color newColor)
        {
            return false;
        }

        public bool SetName(int playerIndex, string newName)
        {
            if (_playerNames == null)
                createPlayerNames();

            _playerNames[playerIndex] = newName;
            return true;
        }

        private void createPlayerNames()
        {
            _playerNames = new string[_players.Count];
            int count = 0;
            foreach (var item in _players)
            {
                _playerNames[count++] = item.displayName;
            }
        }

        public void LoadMap()
        {
            SceneManager.LoadScene(_sceneMap);
        }
    }
}