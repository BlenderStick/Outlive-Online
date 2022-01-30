using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Outlive.Controller
{
    [AddComponentMenu("Outlive/Player/Controller")]
    public class PlayerController : MonoBehaviour, Generic.IPlayerController
    {
        [SerializeField] private PlayerInput _input;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _cameraRange;
        private IList<object> _inputDisableControl;
        public void DisableInputs(object obj)
        {
            _inputDisableControl.Add(obj);
            _input.enabled = false;
        }

        public void EnableInputs(object obj)
        {
            if (_inputDisableControl.Contains(obj))
                _inputDisableControl.Remove(obj);
            if (_inputDisableControl.Count == 0)
                _input.enabled = true;
        }

        public bool RayCast(Vector2 pointInScreen, out RaycastHit hit)
        {
            return Physics.Raycast(_mainCamera.ScreenPointToRay(pointInScreen), out hit);
        }

        public bool RayCast(Vector2 pointInScreen, out RaycastHit hit, int layerMask)
        {
            return Physics.Raycast(_mainCamera.ScreenPointToRay(pointInScreen), out hit, layerMask);
        }

        public bool RayCast(Vector2 pointInScreen, out Collider collider, int layerMask)
        {
            RaycastHit hit;
            if (RayCast(pointInScreen, out hit, layerMask))
            {
                collider = hit.collider;
                return true;
            }
            collider = null;
            return false;
        }

        public bool RayCast(Vector2 pointInScreen, out Collider collider)
        {
            RaycastHit hit;
            if (RayCast(pointInScreen, out hit))
            {
                collider = hit.collider;
                return true;
            }
            collider = null;
            return false;
        }

        public bool RayCast(Vector2 pointInScreen, out Vector3 worldPoint, int layerMask)
        {
            RaycastHit hit;
            if (RayCast(pointInScreen, out hit, layerMask))
            {
                worldPoint = hit.point;
                return true;
            }
            worldPoint = _mainCamera.ScreenPointToRay(pointInScreen).direction * _cameraRange + _mainCamera.transform.position;
            return false;
        }

        public bool RayCast(Vector2 pointInScreen, out Vector3 worldPoint)
        {
            RaycastHit hit;
            if (RayCast(pointInScreen, out hit))
            {
                worldPoint = hit.point;
                return true;
            }
            worldPoint = _mainCamera.ScreenPointToRay(pointInScreen).direction * _cameraRange + _mainCamera.transform.position;
            return false;
        }

        // Start is called before the first frame update
        void Start()
        {
            _input = GetComponent<PlayerInput>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}