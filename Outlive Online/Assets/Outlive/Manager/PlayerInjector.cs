using System.Linq;
using System.Collections.Generic;
using Outlive.Manager.Generic;
using UnityEngine;
using static Outlive.Manager.GameManager;
using Outlive.Unit.Generic;

namespace Outlive.Manager
{
    [ExecuteInEditMode]
    public class PlayerInjector : MonoBehaviour
    {
        [SerializeField] private bool _executeAfterPlay = true;
        [SerializeField] private List<Object> _objectsToInjectPlayer;
        [SerializeField] private GameManager _manager;

        private void Awake() {
            if (Application.isPlaying)
                return;

            if (_executeAfterPlay && _manager != null)
                _manager.CheckPlayersChange(true);
        }

        private void NotifyGameManager()
        {
            if (_manager == null)
                return;
            
            GameObject[] go = FindObjectsOfType<GameObject>();
            foreach (var item in go)
            {
                ICommandableUnit commandable;
                if (item.TryGetComponent(out commandable))
                {
                    IPlayer player = commandable.player;
                    
                    if (player == null)
                        continue;
                        
                    _manager.UnitNotify(item, player);
                }
            }
        }

        public void AddInjectable(UnityEngine.Object target)
        {
            if (!_objectsToInjectPlayer.Contains(target))
                if (target is IPlayerInjectable injectable)
                {
                    _objectsToInjectPlayer.Add(target);
                    injectable.OnInjectorSet(this);
                }
        }
        public void RemoveInjectable(UnityEngine.Object target)
        {
            _objectsToInjectPlayer.Remove(target);
        }

        ///<summary> Procura todos os objetos que implementam IPlayerInjectable na cena </summary>
        public void FindAll()
        {
            _objectsToInjectPlayer.Clear();
            IEnumerable<IPlayerInjectable> injectables = FindObjectsOfType(typeof(Object)).OfType<IPlayerInjectable>();
            foreach (var item in injectables)
            {
                _objectsToInjectPlayer.Add(item as Object);
                item.OnInjectorSet(this);
            }

            UpdateManager();
        }

        public void UpdateInjectable(IPlayerInjectable injectable, string playerName)
        {
            if (playerName == null)
                return;

            Player player = _manager.GetPlayerInEditor(playerName);

            if (player == null)
                return;

            injectable.OnInjectablePlayerChange(_manager, player.displayName, player.displayName, player.color, player.color);
        }

        public void UpdateManager(IPlayerInjectable injectable)
        {
            if (injectable == null || _manager == null)
                return;

            if (injectable is Object obj)
                if (_objectsToInjectPlayer.Contains(obj))
                    _manager.FirePlayerListChange();
        }

        void UpdateManager()
        {
            if (_manager == null)
                return;

            _manager.FirePlayerListChange();
        }

        public void OnPlayerListChange(PlayerListChangeCallback ctx)
        {
            foreach (var item in _objectsToInjectPlayer)
            {
                if (item is IPlayerInjectable injectable)
                    injectable.OnInjectablePlayerListChange(ctx.manager, ctx.players);
            }
        }

        public void OnPlayerNameChange(PlayerChangeCallback ctx)
        {
            foreach (var item in _objectsToInjectPlayer)
            {
                if (item is IPlayerInjectable injectable)
                    injectable.OnInjectablePlayerChange(ctx.manager, ctx.lastName, ctx.currentName, ctx.lastColor, ctx.currentColor);
            }
        }
        
        public void OnGameStart(IGameManager gameManager)
        {
            foreach (var item in _objectsToInjectPlayer)
            {
                if (item is IPlayerInjectable injectable)
                    injectable.OnGameManagerStart(gameManager);
            }
            NotifyGameManager();
        }

    }
}