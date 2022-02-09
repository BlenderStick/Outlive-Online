using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DependencyInjection
{
    public interface DependencyRegister
    {
        bool CanRegister(DependencyListener listener);
        void ClearListeners();
    }

}
