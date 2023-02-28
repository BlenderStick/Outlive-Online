using System;

namespace Outlive.Unit.Behaviour
{
    public interface IMoveBehaviour : IBehaviour
    {
        event Action<IMoveBehaviour, MoveBehaviourState> OnMoveStateChange;
        bool IsMoving{get;}
    }
    public enum MoveBehaviourState
    {
        Moving,
        Stoped
    }
}