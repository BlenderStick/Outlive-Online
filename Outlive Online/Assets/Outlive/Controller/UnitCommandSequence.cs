using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Controller
{
    public class UnitCommandSequence : IDisposable
    {

        public GameObject unit {get; private set;}

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}