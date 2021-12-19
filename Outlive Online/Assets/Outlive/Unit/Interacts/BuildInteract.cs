using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Human.Generic;
using Outlive.Unit.Behaviour;
using Outlive.Unit.Command;
using UnityEngine;

namespace Outlive.Unit.Interacts
{
[CreateAssetMenu(fileName = "BuildInteract", menuName = "Interações/Construir"), Serializable]
    public class BuildInteract : IUnitInteract
    {
        public override bool Command(GameObject source, GameObject target, out ICommand command)
        {
            IConstructableHandler build;
            if (source.GetComponent<IConstructorHandler>() != null && target.TryGetComponent<IConstructableHandler>(out build))
            {
                command = new BuildCommand(build);
                return true;
            }
            command = null;
            return false;
        }

        public override bool Interact(GameObject source, GameObject target, out IBehaviour behaviour)
        {
            IConstructableHandler build;
            if (source.GetComponent<IConstructorHandler>() != null && target.TryGetComponent<IConstructableHandler>(out build))
            {
                behaviour = new BuildBehaviour();
                return true;
            }
            else
            {
                behaviour = null;
                return false;
            }
        }
    }
}