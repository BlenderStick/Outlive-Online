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
        private IList<ICommandController> commandControllers = new List<ICommandController>();


        public void OnInteract(PlayerController.CallbackContext ctx)
        {
            if (ctx.focus == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(ctx.controller.Camera.ScreenPointToRay(ctx.mousePosition), out hit, Mathf.Infinity, _streetLayer))
                {
                    MoveCommand(ctx.controller.Selection, hit.point);
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

        public void MoveCommand(Selection selection, Vector3 target)
        {

            if (selection.Count == 0)
                return;

            Vector2Int target2d = new Vector2Int(Decimal.ToInt32(Decimal.Round(new Decimal(target.x))), Decimal.ToInt32(Decimal.Round(new Decimal(target.z))));
            Vector2Int[] positions = OutliveUtilites.CalculatePointsAroundInGrid(target2d, selection.Count, _map.GetLayers("builds", "jazidas", "obstacles"));

            int ii = 0;
            foreach (var item in selection.Selected)
            {
                // Vector3 experiment = new Vector3(positions[ii].x, 0, positions[ii++].y);
                Vector3 experiment = FromSkyToFloor(positions[ii++], 100f);
                item.GetComponent<ICommandableUnit>().PutCommand(new MoveCommand(experiment), false);
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