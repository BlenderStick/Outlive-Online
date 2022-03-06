using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Unit.Command
{
    public class AttackCommand : ICommand
    {

        Vector3 coordinates;

        public event EventHandler<ICommand> OnStart;
        public event EventHandler<ICommand> OnSkip;

        public AttackCommand(float x, float y, float z) : this(new Vector3(x, y, z))
        {}

        public AttackCommand(Vector3 coordinates){
            this.coordinates = coordinates;
        }

        public Vector3 getCoordinates(){
            return coordinates;
        }

        public void Skip() => OnSkip?.Invoke(this, this);

        public void Start() => OnStart?.Invoke(this, this);

        public object alvo 
        {
            get
            {
                return coordinates;
            }
        }
    }
}

