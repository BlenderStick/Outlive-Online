using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Linq;
using System;
using System.Timers;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using Outlive.Controller.Generic;
using Outlive.Unit.Command;
using Outlive.Unit.Generic;
using UnityEngine;
using Outlive.Grid;
using Outlive.Human.Generic;

namespace Outlive.Controller
{
    public class PlayerCommander : MonoBehaviour
    {
        [SerializeField] private LayerMask _streetLayer;
        [SerializeField] private GridMap _map;
        private IList<ICommandTracker> _commandManagers;

        private void Awake() 
        {
            _commandManagers = new List<ICommandTracker>();
        }

        public void RequestCalcule(ICommandTracker commandManager)
        {
            if (!_commandManagers.Contains(commandManager))
                _commandManagers.Add(commandManager);
        }
        public void CancelRequest(ICommandTracker commandManager)
        {
            _commandManagers.Remove(commandManager);
        }

        private void Update() 
        {
            foreach (var item in _commandManagers)
            {
                item.Calcule();
            }
            _commandManagers.Clear();
        }


        public void OnInteract(PlayerController.CallbackContext ctx)
        {
            if (ctx.focus == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(ctx.controller.Camera.ScreenPointToRay(ctx.mousePosition), out hit, Mathf.Infinity, _streetLayer))
                {
                    MoveCommand(ctx.controller.Selection, hit.point, ctx.isMultiselect);
                }
            }
        }

        void OnSelectionChange(PlayerAction.CallbackContext ctx)
        {

        }

        public class CallbackContext
        {
            public Selection selection {get;}
            public Dictionary<string, ICollection<GameObject>> separatedSelection {get;}
            public PlayerCommander commander {get;}
        }

        public void Command(IEnumerable<GameObject> selection, string commandName, params object[] args)
        {

        }

        public void MoveCommand(Selection selection, Vector3 target, bool isSequence)
        {

            if (selection.Count == 0)
                return;

            Vector2Int target2d = Vector2Int.RoundToInt(target.To2D());
            MoveCommandTracker moveCommandManager = new MoveCommandTracker(target2d, selection.Count, _map.GetLayers("builds", "jazidas", "obstacles").Contains, (v) => FromSkyToFloor(v, 100f));
            moveCommandManager.Calcule();
            
            foreach (var item in selection.Selected)
            {
                ICommandableUnit commandable = item.GetComponent<ICommandableUnit>();
                if (commandable == null)
                    continue;

                MoveCommand command;
                if (moveCommandManager.CanUseCommand(commandable.CanExecuteCommand, out command))
                    commandable.PutCommand(command, isSequence);
            }
        }

        public void BuildCommand(Selection selection, Vector3 target, IConstructable constructable, bool isSequence)
        {
            BuildCommandTracker commandManager = new BuildCommandTracker(
                v => constructable.PositionsToBuild.Contains(Vector2Int.RoundToInt(v)), 
                selection.Count,
                v => _map.Contains(Vector2Int.FloorToInt(v), "builds", "jazidas", "obstacles"));

            commandManager.Project = v => OutliveUtilites.Project(100f, _streetLayer, v);
            commandManager.Target = Vector2Int.RoundToInt(target.To2D());
            
            commandManager.OnProgress += (o, c) =>
            {
                if (constructable.AddBuildProgress(c.ConstructorPosition, c.Value))
                {
                    if (!constructable.NeedBuild)
                        commandManager.Cancel();
                }
            };

            commandManager.Calcule();

            foreach (var item in selection.Selected)
            {
                ICommand command;
                ICommandableUnit commandable;
                if (item.TryGetComponent(out commandable) && commandManager.CanUseCommand(commandable.CanExecuteCommand, out command))
                {
                    commandable.PutCommand(command, isSequence);
                }
            }
        }

        public Vector3 FromSkyToFloor(Vector2 input, float high)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(input.x, high, input.y), Vector3.down, out hit, Mathf.Infinity, _streetLayer))
            {
                return hit.point;
            }
            return input;
        }
    }
}