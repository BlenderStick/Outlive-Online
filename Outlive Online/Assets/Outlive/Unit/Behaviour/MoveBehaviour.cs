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
            lastVector = obj.transform.position;
        }

        public bool UpdateBehaviour(GameObject obj, ICommand command, bool cancel = false)
        {
            if (lastVector != (Vector3) command.alvo)
            {
                lastVector = (Vector3) command.alvo;
                navigation.destination = lastVector;
                return true;
            }
            
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
    }
}