using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DependencyInjection
{
    public class DependencyInjection : MonoBehaviour
    {

        public static DependencyInjection current {get; private set;}

        private List<DependencyRegister> _registers;
        [SerializeField] private RegisterItem[] _object_registers;

        private void Awake() 
        {
            _registers = new List<DependencyRegister>();
            foreach (var item in _object_registers)
                if (item.register is DependencyRegister register)
                    _registers.Add(register);

            if (current != null)
                current.AddRegister(_registers);
            else
                current = this;
        }

        private void OnDestroy() 
        {
            if (current == this)
                current = null;
        }

        internal void ForceRegisterAllInEdit(bool clearFirst = false)
        {
            Object[] listeners = FindObjectsOfType(typeof(DependencyListener));
            foreach (var register in _object_registers)
            {
                if (register.register is DependencyRegister dr)
                {
                    if (clearFirst)
                        dr.ClearListeners();
                    foreach (var listener in listeners)
                    {
                        if (listener is DependencyListener l)
                            if (dr.CanRegister(l))
                            {
                                l.registerEvents(dr);
                            }
                    }
                }
            }
        }

        public void Unregister(DependencyListener listener)
        {
            if (Application.isPlaying)
            {
                if (current == this)
                    foreach (var register in _object_registers)
                    {
                        if (register.register is DependencyRegister dr)
                        {
                            listener.unregisterEvents(dr);
                        }
                    }
                else
                    current.Unregister(listener);
            }
            else
            {
                foreach (var register in _registers)
                {
                    if (register is DependencyRegister dr)
                    {
                        listener.unregisterEvents(dr);
                    }
                }
            }
        }

        public void ForceRegisterAll(bool clearFirst = false)
        {
            if (current != this)
            {
                current.ForceRegisterAll(clearFirst);
                return;
            }

            Object[] listeners = FindObjectsOfType(typeof(DependencyListener));
            foreach (var register in _registers)
            {
                if (clearFirst)
                    register.ClearListeners();
                foreach (var listener in listeners)
                {
                    if (listener is DependencyListener l)
                        if (register.CanRegister(l))
                        {
                            l.registerEvents(register);
                        }
                }
            }
        }
        public void AddRegister(DependencyRegister register)
        {
            if (current == this)
                _registers.Add(register);
            else
                current.AddRegister(register);
        } 
        public void AddRegister(IEnumerable<DependencyRegister> register)
        {
            if (current == this)
                _registers.AddRange(register);
            else
                current.AddRegister(register);
        }
        public void RemoveRegister(DependencyRegister register)
        {
            if (current == this)
                _registers.Remove(register);
            else
                current.RemoveRegister(register);
        }
    }
}