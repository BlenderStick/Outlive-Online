using System.Collections;
using System.Collections.Generic;
using Outlive.Behaviours;
using Outlive.Unit.Behaviour;
using Outlive.Unit.Command;
using Outlive.Unit.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Outlive.Unit
{
    public class UnitMove : UnitGeneric
    {
        public override string UnitName => Outlive.PrefabsName.HM_CONSTRUCTOR;

        public UnityEvent<MoveCallback> OnMove;
        public UnityEvent<BuildCallback> OnBuild;
        public UnityEvent OnMovingStop;

        private bool _started;
        private BuildBehaviour _buildBehaviour;
        private MoveBehaviour _moveBehaviour;

        protected override bool TryGetBehaviour(ICommand command, out IBehaviour behaviour)
        {
            if (!_started)
            {
                _moveBehaviour = new MoveBehaviour();
                _buildBehaviour = new BuildBehaviour(_moveBehaviour);

                _moveBehaviour.OnMoveStateChange += OnMoveStateChange;
                _buildBehaviour.OnStateChange += OnBuildStateChange;
                _started = true;
            }

            if (command is BuildCommand)
            {
                behaviour = _buildBehaviour;
                return true;
            }
            if (command is MoveCommand)
            {
                behaviour = _moveBehaviour;
                return true;
            }

            // if (Behaviour is Behaviour.BuildBehaviour build)
            //     build.OnStateChange -= OnBuildStateChange;

            // if (command is BuildCommand)
            // {
            //     IBehaviour b;
            //     if (BehaviourPreset.TryCreateBehaviour(BehaviourPreset.Behaviour_Build, out b))
            //     {
            //         (b as Behaviour.BuildBehaviour).OnStateChange += OnBuildStateChange;
            //         (b as Behaviour.BuildBehaviour).OnMoveStateChange += OnMoveStateChange;
            //         behaviour = b;
            //         return true;
            //     }
            // }
            // if (command is MoveCommand)
            // {
            //     MoveBehaviour moveBehaviour = new MoveBehaviour();
            //     moveBehaviour.OnMoveStateChange += OnMoveStateChange;
            //     behaviour = moveBehaviour;
            //     return true;
            // }
                
            behaviour = null;
            return false;
        }
        public override bool CanExecuteCommand(string commandName)
        {
            return commandName == BehaviourPreset.Behaviour_Move || commandName == BehaviourPreset.Behaviour_Build;
        }

        public override void UpdateAnimator(bool isBehaviourChanged)
        {
        }

        protected override void OnBehaviourChange(IBehaviour lastBehaviour, IBehaviour currentBehaviour)
        {

        }


        private void OnMoveStateChange(IMoveBehaviour behaviour, MoveBehaviourState state) => OnMove.Invoke(new MoveCallback(this, state));

        private void OnBuildStateChange(Behaviour.BuildBehaviour.CallbackContext ctx) => OnBuild.Invoke(new BuildCallback(this, ctx.State));

    }

    public class BuildCallback
    {
        public ICommandableUnit Unit;
        public BuildBehaviourState State;

        public BuildCallback(ICommandableUnit unit, BuildBehaviourState state)
        {
            Unit = unit;
            State = state;
        }
    }

    public class MoveCallback
    {
        public ICommandableUnit Unit;
        public MoveBehaviourState State;

        public MoveCallback(ICommandableUnit unit, MoveBehaviourState state)
        {
            Unit = unit;
            State = state;
        }
    }

}