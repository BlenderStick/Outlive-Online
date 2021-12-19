using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Unit.Command;
using UnityEngine;
using UnityEngine.AI;

namespace Outlive.Unit.Behaviour
{
    [CreateAssetMenu(fileName = "MoveBehaviour", menuName = "Behaviour/Move"), Serializable]
    public class MoveBehaviour : BasicBehaviour
    {

        private GameObject gameObject;
        private NavMeshAgent navigation;

        public override void Cancel(GameObject obj, ICommand command)
        {
            navigation.isStopped = true;
        }

        public override bool Condition(ICommand command)
        {
            return command is MoveCommand;
        }

        public override void Reset()
        {
            gameObject = null;
            navigation = null;
        }

        public override void Setup(GameObject obj)
        {
            gameObject = obj;
            if (!obj.TryGetComponent<NavMeshAgent>(out navigation))
                throw new System.Exception("O objeto não possui um NavMeshAgent Component");
        }

        public override bool UpdateBehaviour(GameObject obj, ICommand command)
        {
            if (obj == gameObject)
            {
                Debug.Log(command.alvo);
                if (!navigation.destination.Equals(command.alvo))
                    navigation.destination = (Vector3) command.alvo;
                
                if (navigation.remainingDistance < 0.1f)
                {
                    navigation.isStopped = true;
                    return false;
                }
                else
                {
                    navigation.isStopped = false;
                    return true;
                }
            }
            return false;
        }
    }
}