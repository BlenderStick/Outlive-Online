using System.Collections;
using System.Collections.Generic;
using Outlive.Behaviours;
using Outlive.Unit.Command;
using UnityEngine;

namespace Outlive.Unit
{
    public class UnitMove : UnitGeneric
    {
        public override string UnitName => Outlive.Units.HM_CONSTRUCTOR;

        protected override bool TryCreateBehaviour(ICommand command, out IBehaviour behaviour)
        {
            if (command is MoveCommand)
                return BehaviourPreset.CreateBehaviour(BehaviourPreset.Behaviour_Move, out behaviour);
            behaviour = null;
            return false;
        }
    }
}