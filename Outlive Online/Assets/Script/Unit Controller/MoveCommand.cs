using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : PlayerCommand
{

    Vector2 coordinates;


    public MoveCommand(float x, float y) : this(new Vector2(x, y))
    {
        
    }

    public MoveCommand(Vector2 coordinates){
    this.coordinates = coordinates;
    }

    public override object alvo {
        get {
            return coordinates;
        }
    }


}
