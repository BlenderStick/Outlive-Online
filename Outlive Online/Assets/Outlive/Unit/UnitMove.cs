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
            if (Behaviour is Behaviour.BuildBehaviour build)
                build.OnStateChange -= OnStateChange;

            if (command is MoveCommand)
                return BehaviourPreset.CreateBehaviour(BehaviourPreset.Behaviour_Move, out behaviour);
            if (command is BuildCommand)
            {
                IBehaviour b;
                if (BehaviourPreset.CreateBehaviour(BehaviourPreset.Behaviour_Build, out b))
                {
                    (b as Behaviour.BuildBehaviour).OnStateChange += OnStateChange;
                    behaviour = b;
                    return true;
                }
            }
                
            behaviour = null;
            return false;
        }
        public override bool CanExecuteCommand(string commandName)
        {
            return commandName == BehaviourPreset.Behaviour_Move || commandName == BehaviourPreset.Behaviour_Build;
        }

        private void OnStateChange(Behaviour.BuildBehaviour.CallbackContext ctx)
        {
            Debug.Log(ctx.State);
        }
    }
}