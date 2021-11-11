using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : PlayerCommand
{

    Vector3 coordinates;

    public AttackCommand(float x, float y, float z) : this(new Vector3(x, y, z))
    {}

    public AttackCommand(Vector3 coordinates){
        this.coordinates = coordinates;
    }

    public Vector3 getCoordinates(){
        return coordinates;
    }

    public override object alvo 
    {
        get
        {
            return coordinates;
        }
    }
}
