using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGroup
{
    private UnitBehaviour[] units;

    public UnitGroup(UnitBehaviour[] units)
    {
        this.units = units;
    } 

    public void removeUnits(UnitBehaviour[] oldUnits)
    {

    }

    public PlayerCommand[] GetCommands()
    {
        return null;
    }

    public void putCommand(PlayerCommand command, bool enfilerate)
    {

    }
}
