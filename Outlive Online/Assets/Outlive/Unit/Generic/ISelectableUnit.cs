using System.Collections;
using System.Collections.Generic;
using Outlive.Unit.Command;
using UnityEngine;

namespace Outlive.Unit.Generic
{
    public interface ISelectableUnit
    {

        void UnitSelect();
        void UnitDeselect();
        void UnitHover();
        void UnitNotHover();
    }
}

