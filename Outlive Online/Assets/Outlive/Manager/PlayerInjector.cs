using System.Runtime.CompilerServices;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using Outlive.Manager.Generic;
using UnityEngine;
using static Outlive.Manager.GameManager;
using Outlive.Unit.Generic;
using Outlive.Controller;
using Outlive.Unit;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Outlive.Manager
{
    [ExecuteInEditMode]
    public class PlayerInjector : MonoBehaviour
    {
        [SerializeField] private bool _executeAfterPlay = true;
        [SerializeField] private GameManager _manager;
        [SerializeField] private List<Object> _objectsToInjectPlayer;

        private void Awake() {
            if (Application.isPlaying)
                return;
        }

        private void Start() {
            NotifyGameManager();
        }

        ///<summary>Notifica o GameManager sobre todos os objetos criados em cena</summary>
        private void NotifyGameManager()
        {
            if (_manager == null)
                return;
            
            GameObject[] go = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
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

        public void FindAll()
        {

            _objectsToInjectPlayer.Clear();

            foreach (var item in FindObjectsByType<Object>(FindObjectsSortMode.None))
            {
                if (item is IPlayerInjectable)
                    _objectsToInjectPlayer.Add(item);
            }
            _manager.FirePlayerListChange();
        }

        public void OnPlayerListChange(PlayerListChangeCallback ctx)
        {
            foreach (var item in _objectsToInjectPlayer)
            {
                if (item is IPlayerInjectable injectable)
                    injectable.OnLoadPlayerListChange(ctx.manager, ctx.players);
            }
        }

        internal void AddInjectable(Object injectable)
        {
            if (_objectsToInjectPlayer.Contains(injectable))
                return;
                
            if (injectable is IPlayerInjectable)
                _objectsToInjectPlayer.Add(injectable);
        }

        internal void RemoveInjectable(Object injectable)
        {
            _objectsToInjectPlayer.Remove(injectable);
        }

        internal void ClearInjectables()
        {
            foreach (var item in _objectsToInjectPlayer)
            {
                if (item is IPlayerInjectable injectable)
                    injectable.OnLoadPlayerListChange(_manager, new Player[0]);
            }
            _objectsToInjectPlayer.Clear();
        }
    }
}