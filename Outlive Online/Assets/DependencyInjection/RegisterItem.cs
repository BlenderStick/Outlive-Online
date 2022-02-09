using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DependencyInjection
{
    [Serializable]
    public class RegisterItem
    {
        [SerializeField] private UnityEngine.Object _register;
        public UnityEngine.Object register => _register;
    }
}