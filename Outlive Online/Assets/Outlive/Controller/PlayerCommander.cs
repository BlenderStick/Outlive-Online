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

namespace Outlive.Controller
{
    public class PlayerCommander : MonoBehaviour
    {
        [SerializeField] private LayerMask _streetLayer;
        [SerializeField] private GridMap _map;
        private IList<ICommandManager> _commandManagers;

        private void Awake() 
        {
            _commandManagers = new List<ICommandManager>();
        }

        public void RequestCalcule(ICommandManager commandManager)
        {
            if (!_commandManagers.Contains(commandManager))
                _commandManagers.Add(commandManager);
        }
        public void CancelRequest(ICommandManager commandManager)
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

            Vector2Int target2d = new Vector2Int(Decimal.ToInt32(Decimal.Round(new Decimal(target.x))), Decimal.ToInt32(Decimal.Round(new Decimal(target.z))));
            MoveCommandManager moveCommandManager = new MoveCommandManager(target2d, _map.GetLayers("builds", "jazidas", "obstacles").Contains, (v) => FromSkyToFloor(v, 100f));
            
            foreach (var item in selection.Selected)
            {
                ICommandableUnit commandable = item.GetComponent<ICommandableUnit>();
                if (commandable == null)
                    continue;

                int max = selection.Count;
                MoveCommand command;
                if (moveCommandManager.CanUseCommand(commandable.CanExecuteCommand, out command))
                {
                    command.OnStart += (o, c) =>
                    {
                        moveCommandManager.EnterCommand(command);
                        RequestCalcule(moveCommandManager);
                    };
                    command.OnSkip += (o, c) =>
                    {
                        moveCommandManager.Count++;
                        if (moveCommandManager.Count == max)
                        {
                            moveCommandManager.Dispose();
                            CancelRequest(moveCommandManager);
                        }
                        else
                            RequestCalcule(moveCommandManager);
                    };

                }
                commandable.PutCommand(command, isSequence);
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