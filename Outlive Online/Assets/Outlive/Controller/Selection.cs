using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Outlive.Controller.Generic;
using UnityEngine;

namespace Outlive.Controller
{
    public class Selection
    {
        private Action<GameObject, Selection, bool> _onChange;

        private List<GameObject> _selection;
        private ReaderWriterLock _rwlSelection;

        public IEnumerable<GameObject> Selected 
        {
            get
            {
                _rwlSelection.AcquireReaderLock(5000);
                foreach (var item in _selection)
                {
                    yield return item;
                }
                _rwlSelection.ReleaseReaderLock();
            }
        }
        public int Count => _selection.Count;

        public Selection()
        {
            _selection = new List<GameObject>();
            _rwlSelection = new ReaderWriterLock();
        }
        public Selection(Action<GameObject, Selection, bool> onChange) : this()
        {
            _onChange = onChange;
        }


        public void Add(params GameObject[] units) => Add((IEnumerable<GameObject>) units);
        public void Add(IEnumerable<GameObject> units)
        {
            _rwlSelection.AcquireWriterLock(5000);
            _selection.AddRange(units);
            _rwlSelection.ReleaseWriterLock();
            if (_onChange != null)
                foreach (var item in units)
                    if (item != null)
                        _onChange.Invoke(item, this, true);
        }
        public void Remove(GameObject unit) => Remove(_selection.IndexOf(unit));
        public void Remove(int index)
        {
            if (index > _selection.Count || index < 0)
                return;

            GameObject lostObject = _selection[index];
            _rwlSelection.AcquireWriterLock(5000);
            _selection.RemoveAt(index);
            _rwlSelection.ReleaseWriterLock();
            fireOnSelectionChangeLost(lostObject);
        }
        public void Clear()
        {
            GameObject[] oldObjects = _selection.ToArray();
            _rwlSelection.AcquireWriterLock(5000);
            _selection.Clear();
            _rwlSelection.ReleaseWriterLock();
            foreach (var item in oldObjects)
                fireOnSelectionChangeLost(item);
        }

        public bool Contain(GameObject unit)
        {
            _rwlSelection.AcquireReaderLock(5000);
            bool result = _selection.Contains(unit);
            _rwlSelection.ReleaseReaderLock();

            return result;
        }

        private void fireOnSelectionChangeLost(GameObject current)
        {
            if (current == null)
                return;

            if (_onChange != null)
                _onChange.Invoke(current, this, false);
        }
    }
}