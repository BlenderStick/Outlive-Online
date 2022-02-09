using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DependencyInjection
{
    public interface DependencyListener
    {
        void registerEvents(DependencyRegister register);
        void unregisterEvents(DependencyRegister register);
    }
}