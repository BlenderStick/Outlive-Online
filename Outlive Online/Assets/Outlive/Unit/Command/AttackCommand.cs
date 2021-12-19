using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Unit.Command
{
    public class AttackCommand : ICommand
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

        public object alvo 
        {
            get
            {
                return coordinates;
            }
        }
    }
}

