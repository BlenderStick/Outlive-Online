using System.Data.SqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using Outlive.Unit.Command;
using UnityEngine;
using Outlive.Unit.Behaviour;

namespace Outlive.Behaviours
{
    public static class BehaviourPreset
    {

        private static Dictionary<string, Func<IBehaviour>> behaviours {get; set;}

        public const string Behaviour_Move = "move";
        public const string Behaviour_Build = "build";
    }
}