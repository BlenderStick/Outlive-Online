using System.Data.Common;
using System;
using Outlive.Human.Generic;
using UnityEngine;

namespace Outlive.Unit.Command
{
    public class BuildCommand : MoveCommand, IDisposable
    {
        private Func<Vector2, int, bool> _addProgress;
        public BuildCommand(Func<Vector2, int, bool> addProgressCommand)
        {
            if (addProgressCommand == null)
                throw new ArgumentNullException();

            _addProgress = addProgressCommand;
        }
        public Quaternion Angle {get; set;}

        public bool CheckChegou(Vector3 position)
        {
            return (position - Target).sqrMagnitude < 0.1f;
        }

        public BuildState CheckState(Vector3 position, bool building)
        {
            if (IsDone)
                return BuildState.Completed;
            else if (CheckChegou(position))
            {
                if (!building)
                    return BuildState.CanBuild;
            }
            return BuildState.None;
        }

        public bool AddProgress(Vector3 position, int value) => _addProgress(position.To2D(), value);
    }

    public enum BuildState
    {
        None,
        CanBuild,
        Completed
    }
}