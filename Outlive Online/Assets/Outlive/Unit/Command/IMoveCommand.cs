

using UnityEngine;

namespace Outlive.Unit.Command
{
    public interface IMoveCommand : ICommand
    {
        Vector3 Target {get;}

        MoveState CheckState(Vector3 position);
    }

    public enum MoveState
    {
        None,
        TargetChanged,
        Completed
    }
}