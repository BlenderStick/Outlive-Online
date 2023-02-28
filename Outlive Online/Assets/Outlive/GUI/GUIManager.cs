using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Controller;
using Outlive.Grid;
using Outlive.GUI.Generic;
using Outlive.Unit.Generic;
using UnityEngine;

namespace Outlive.GUI
{
    public class GUIManager : MonoBehaviour
    {

        public class CallbackContext
        {
            public CallbackContext(RectTransform root, GUIManager guiManager, Selection selection, PlayerController controller, GridMapInteract gridInteract, GridMap gridMap, PlayerCommander playerCommander)
            {
                Root = root;
                GuiManager = guiManager;
                Selection = selection;
                Controller = controller;
                GridInteract = gridInteract;
                GridMap = gridMap;
                PlayerCommander = playerCommander;
            }

            public RectTransform Root {get; private set;}
            public GUIManager GuiManager {get; private set;}
            public Selection Selection {get; private set;}
            public PlayerController Controller {get; private set;}
            public GridMapInteract GridInteract {get; private set;}
            public GridMap GridMap {get; private set;}
            public PlayerCommander PlayerCommander {get; private set;}
        }

        [SerializeField] private RectTransform _uiTransform;
        [SerializeField] private GridMapInteract _gridInteract;
        [SerializeField] private GridMap _map;
        [SerializeField] private PlayerCommander _playerCommander;
        private IGUILoader _currentGUI;
        private PlayerController _currentPlayerController;
        private Selection _currentSelection;

        public void OnSelectionChange(PlayerAction.CallbackContext ctx)
        {
            if (ctx.selection.Count == 0)
            {
                LeaveCurrent();
                return;
            }

            _currentPlayerController = ctx.controller;
            _currentSelection = ctx.selection;

            IGUIUnit guiUnit = null;

            foreach (var item in _currentSelection.Selected)
                if (item.TryGetComponent(out guiUnit))
                    break;

            if (guiUnit == null)
                return;

            SetGUIPrefab(Outlive.GUILoad.GetGUI(guiUnit.UnitName), ctx.controller, ctx.selection);
        }

        public void SetGUIPrefab(GameObject prefab, PlayerController controller, Selection selection)
        {
            LeaveCurrent();
            GameObject instance = Instantiate(prefab, _uiTransform);
            _currentGUI = instance.GetComponent<IGUILoader>();
            _currentGUI.Load(new CallbackContext(_uiTransform, this, selection, controller, _gridInteract, _map, _playerCommander));
        }
        private void LeaveCurrent()
        {
            if (_currentGUI == null)
                return;

            CallbackContext ctx = new CallbackContext(_uiTransform, this, _currentSelection, _currentPlayerController, _gridInteract, _map, _playerCommander);
            _currentGUI.Leave(ctx);
            _currentGUI = null;
        }
        
    }
}