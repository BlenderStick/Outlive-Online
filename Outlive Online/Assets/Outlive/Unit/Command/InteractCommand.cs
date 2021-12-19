using System;
using UnityEngine;

namespace Outlive.Unit.Command
{
    public class InteractCommand : ICommand
    {
        private GameObject obj;
        public InteractCommand(GameObject target)
        {
            if (target == null)
                throw new ArgumentNullException("target não pode ser nulo");
            obj = target;
        }
        public object alvo 
        {
            get
            {
                return obj;
            }
        }
        public GameObject target
        {
            get
            {
                return obj;
            }
        }
    }
}