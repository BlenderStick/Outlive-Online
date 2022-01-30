using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.GUI
{
    public class GUIManager : MonoBehaviour
    {

        public class Event : Generic.IGUILoaderEvent
        {
            private GUIManager manager;
            internal Event(GUIManager guiManager)
            {
                manager = guiManager;
            }
            public RectTransform root => manager._uiTransform;

            public Generic.IGUILoader current {get => manager.guiLoader; set => manager.guiLoader = value;}
        }

        [SerializeField] private Object _object_order;
        [SerializeField] private RectTransform _uiTransform;

        private IList<Generic.IGUILoader> _loaderList;
        private Generic.IGUILoader _guiLoader;
        private Event evt;
        public Generic.IGUILoader guiLoader
        {
            get => _guiLoader;
            set
            {
                if (_guiLoader != null)
                    _guiLoader.leave(evt);

                _guiLoader = value;

                _guiLoader.load(evt);
            }
        }
        public Generic.IGUILoaderOrder guiLoaderOrder {get; set;}

        public void SetGUILoaders(params Generic.IGUILoader[] loaders)
        {
            List<Generic.IGUILoader> tempLoaders = new List<Generic.IGUILoader>();
            foreach (var item in loaders)
            {
                if (item == null || tempLoaders.Contains(item))
                    continue;
                tempLoaders.Add(item);
            }
            System.GC.ReRegisterForFinalize(_loaderList);

            if (guiLoaderOrder == null)
                _loaderList = new List<Generic.IGUILoader>(tempLoaders);
            else
                _loaderList = new List<Generic.IGUILoader>(guiLoaderOrder.order(tempLoaders));

            if (_loaderList.Contains(_guiLoader))
                return;

            if (_loaderList.Count > 0)
                guiLoader = _loaderList[0];
            else
                guiLoader = null;

        }

        public void NextLoader()
        {
            if (_loaderList == null)
                return;
                
            int index = _loaderList.IndexOf(guiLoader);
            if (index < _loaderList.Count)
                guiLoader = _loaderList[index++];
            else
                guiLoader = _loaderList[0];
        }

        private void Awake() 
        {
            if (_object_order is Generic.IGUILoaderOrder iOrder)
                guiLoaderOrder = iOrder;
            evt = new Event(this);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}