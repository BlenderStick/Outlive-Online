using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SelectBoxEventListener : MonoBehaviour
{
    [SerializeField] private RectTransform _selectBox;

    public void OnRect(Rect rect)
    {
        _selectBox.gameObject.SetActive(true);
        _selectBox.position = rect.position;
        _selectBox.sizeDelta = rect.size;
    }
    public void OnDone()
    {
        _selectBox.gameObject.SetActive(false);
    }

    // [SerializeField] private UnityEvent<Rect> _onDragRuning;
    // [SerializeField] private UnityEvent<Rect> _onDragDone;
    // [SerializeField] private UnityEvent _onDragCancel;
    // private Vector3 _worldPosition;
    // private Vector3 _cameraNoRotation;
    // private Camera _currentCamera;
    // private bool isCorrectDrag;


    // public void OnBeginDrag(BaseEventData evt)
    // {
    //     if (!UnityEngine.InputSystem.Mouse.current.leftButton.isPressed)
    //         return;

    //     isCorrectDrag = true;
        
    //     _selectBox.gameObject.SetActive(true);
    //     _selectBox.position = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
    //     _selectBox.sizeDelta = Vector2.zero;

    //     RaycastHit hit;
    //     if (Physics.Raycast(_currentCamera.ScreenPointToRay(UnityEngine.InputSystem.Mouse.current.position.ReadValue()), out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("ViewProject")))
    //     {
    //         _worldPosition = hit.point;
    //     }
    // }
    // public void OnDrag(BaseEventData evt)
    // {
    //     if (!isCorrectDrag)
    //         return;

    //     if (UnityEngine.InputSystem.Mouse.current.rightButton.isPressed)
    //     {
    //         isCorrectDrag = false;
    //         _selectBox.gameObject.SetActive(false);
    //         _onDragCancel.Invoke();
    //         return;
    //     }

    //     Vector2 size = UnityEngine.InputSystem.Mouse.current.position.ReadValue() - (Vector2) _selectBox.position;
    //     Vector2 newSize = new Vector2(size.x, size.y);
    //     Vector3 newScale = Vector3.one;
    //     if (size.x < 0)
    //     {
    //         newSize.x = -size.x;
    //         newScale.x = -1;
    //     }
    //     if (size.y < 0)
    //     {
    //         newSize.y = -size.y;
    //         newScale.y = -1;
    //     }
            
    //     _selectBox.sizeDelta = newSize;
    //     _selectBox.localScale = newScale;

    //     _onDragRuning.Invoke(selectArea);
    // }

    // public void OnEndDrag(BaseEventData evt)
    // {
    //     if (!isCorrectDrag)
    //         return;
        
    //     isCorrectDrag = false;
    //     _onDragDone.Invoke(selectArea);
    // }

    // public void OnPointerUp(BaseEventData evt)
    // {
    //     _selectBox.gameObject.SetActive(false);
    // }

    // public void OnCameraMove(Camera camera)
    // {
    //     if (!isCorrectDrag)
    //         return;
            
    //     Vector3 cameraNoRotation = _currentCamera.transform.position;
    //     Vector3 desloc = cameraNoRotation - _cameraNoRotation;
    //     _selectBox.transform.position = camera.WorldToScreenPoint(_worldPosition);
    // }
    // public void OnCameraChange(Camera camera)
    // {
    //     _currentCamera = camera;
    // }

    // private Rect selectArea
    // {
    //     get 
    //     {
    //         Vector2 coord1 = _selectBox.position;
    //         Vector2 coord2 = (Vector2) _selectBox.position + (_selectBox.rect.size * _selectBox.localScale);
    //         return Rect.MinMaxRect(
    //             Mathf.Min(coord1.x, coord2.x), 
    //             Mathf.Min(coord1.y, coord2.y),
    //             Mathf.Max(coord1.x, coord2.x), 
    //             Mathf.Max(coord1.y, coord2.y));
    //     }
    // }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
