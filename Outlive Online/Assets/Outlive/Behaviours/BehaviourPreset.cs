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

        public static bool CreateBehaviour(string name, out IBehaviour behaviour)
        {
            Func<IBehaviour> action;
            bool result = behaviours.TryGetValue(name, out action);
            behaviour = action == null? null: action.Invoke();
            return result;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void SetBehaviours()
        {
            behaviours = new Dictionary<string, System.Func<IBehaviour>>();
            behaviours.Add(Behaviour_Move, () => new MoveBehaviour());
            behaviours.Add(Behaviour_Build, () => new BuildBehaviour());
        }

        private static Dictionary<string, Func<IBehaviour>> behaviours {get; set;}

        public const string Behaviour_Move = "move";
        public const string Behaviour_Build = "build";
    }
}