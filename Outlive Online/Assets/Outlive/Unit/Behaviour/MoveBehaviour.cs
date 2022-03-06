using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Unit.Command;
using UnityEngine;
using UnityEngine.AI;

namespace Outlive.Unit.Behaviour
{
    public class MoveBehaviour : IBehaviour
    {

        private GameObject gameObject;
        private NavMeshAgent navigation;
        private Vector3 lastVector;
        private bool isStart = true;

        public void Dispose()
        {
            gameObject = null;
            navigation = null;
            GC.SuppressFinalize(this);
        }

        public void ForceCancel(GameObject obj, ICommand command)
        {
            navigation.isStopped = true;
        }

        public void Setup(GameObject obj, ICommand command)
        {
            gameObject = obj;
            if (!obj.TryGetComponent<NavMeshAgent>(out navigation))
                throw new System.Exception("O objeto " + obj + " não possui um NavMeshAgent Component");
            MoveCommand moveCommand = command as MoveCommand;
            moveCommand.FireStart();
        }

        public bool UpdateBehaviour(GameObject obj, ICommand command, bool cancel = false)
        {
            MoveCommand moveCommand = command as MoveCommand;
            if (cancel)
            {
                navigation.isStopped = true;
                return false;
            }

            switch (moveCommand.CheckStatus(navigation.nextPosition))
            {
                case MoveStatus.TargetChanged:
                    navigation.destination = moveCommand.Target;
                    navigation.isStopped = false;
                    return true;
                case MoveStatus.Completed:
                    navigation.isStopped = true;
                    return false;
                default:
                    return true;
            }
        }
    }
}