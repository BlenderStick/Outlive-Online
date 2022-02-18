using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Controller;
using Outlive.GUI.Generic;
using Outlive.Unit.Generic;
using UnityEngine;

namespace Outlive.GUI
{
    public class GUIManager : MonoBehaviour
    {

        public class CallbackContext
        {
            public CallbackContext(RectTransform root, GUIManager guiManager, Selection selection, PlayerController controller)
            {
                this.root = root;
                this.guiManager = guiManager;
                this.selection = selection;
                this.controller = controller;
            }

            public RectTransform root {get; private set;}
            public GUIManager guiManager {get; private set;}
            public Selection selection {get; private set;}
            public PlayerController controller {get; private set;}
        }

        [SerializeField] private RectTransform _uiTransform;
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
            Debug.Log(guiUnit.UnitName);

            SetGUIPrefab(Outlive.GUILoad.GetGUI(guiUnit.UnitName), ctx.controller, ctx.selection);
        }

        public void SetGUIPrefab(GameObject prefab, PlayerController controller, Selection selection)
        {
            LeaveCurrent();
            GameObject instance = Instantiate(prefab, _uiTransform);
            _currentGUI = instance.GetComponent<IGUILoader>();
            _currentGUI.Load(new CallbackContext(_uiTransform, this, selection, controller));
        }
        private void LeaveCurrent()
        {
            if (_currentGUI == null)
                return;

            CallbackContext ctx = new CallbackContext(_uiTransform, this, _currentSelection, _currentPlayerController);
            _currentGUI.Leave(ctx);
            _currentGUI = null;
        }
        
    }
}