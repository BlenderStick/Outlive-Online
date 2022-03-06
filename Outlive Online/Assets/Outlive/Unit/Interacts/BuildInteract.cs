using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Human.Generic;
using Outlive.Unit.Behaviour;
using Outlive.Unit.Command;
using UnityEngine;

namespace Outlive.Unit.Interacts
{
    public class BuildInteract : IUnitInteract
    {
        public override bool Command(GameObject source, GameObject target, out ICommand command)
        {
            throw new NotImplementedException();
        }

        public override bool Interact(GameObject source, GameObject target, out IBehaviour behaviour)
        {
            throw new NotImplementedException();
        }
    }
}