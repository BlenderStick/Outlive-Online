using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Outlive.GUI.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Outlive.GUI
{
    [CreateAssetMenu(fileName = "new GUI Loader", menuName = "Outlive/GUI/Loader")]
    public class GUILoader : Generic.GenericGUILoader
    {
        [SerializeField] private GameObject _UIElement;
        private GameObject ui;

        [SerializeField] private UnityEvent<IGUILoaderEvent> _onLoad = new UnityEvent<IGUILoaderEvent>();
        [SerializeField] private UnityEvent<IGUILoaderEvent> _onLeave = new UnityEvent<IGUILoaderEvent>();

        public UnityEvent<IGUILoaderEvent> onLoad => _onLoad;
        public UnityEvent<IGUILoaderEvent> onLeave => _onLeave;

        public override void leave(IGUILoaderEvent evt)
        {
            onLeave.Invoke(evt);
            IUIListener listener;
            if (ui.TryGetComponent(out listener))
                listener.onLeave(evt);
            
            Destroy(ui);
        }

        public override void load(IGUILoaderEvent evt)
        {
            ui = Instantiate(_UIElement, evt.root);
            IUIListener listener;
            if (ui.TryGetComponent(out listener))
                listener.onLoad(evt);
            onLoad.Invoke(evt);
        }

        private class UIDestroyElement : MonoBehaviour
        {
            private void OnDestroy() {
                
            }
        }
    }
}