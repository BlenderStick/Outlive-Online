using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Controller.Generic
{
    public interface ICommandController: IDisposable
    {
        ICollection<UnitCommandSequence> units {get;}
        void Update();

        bool HaveUnits();
    }
}