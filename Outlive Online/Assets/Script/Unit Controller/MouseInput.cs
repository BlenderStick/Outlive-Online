// GENERATED AUTOMATICALLY FROM 'Assets/Script/Unit Controller/MouseInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MouseInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MouseInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MouseInput"",
    ""maps"": [
        {
            ""name"": ""Mouse"",
            ""id"": ""ca58501f-1f21-4134-8772-549c22b71ab3"",
            ""actions"": [
                {
                    ""name"": ""SelectClick"",
                    ""type"": ""Button"",
                    ""id"": ""67bcbef5-7780-4e2f-b698-a543b2d21bf9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7798faf2-8bfe-40c0-b926-8c7f1d1cb8e1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseDrag"",
                    ""type"": ""PassThrough"",
                    ""id"": ""00ae7c8f-70bc-46eb-9b4f-99fe35fad0f6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""CommandClick"",
                    ""type"": ""Button"",
                    ""id"": ""494f2e6e-040d-4b8d-bb84-553ce99cc861"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""KeyAttack"",
                    ""type"": ""Button"",
                    ""id"": ""5987922f-3ca3-43b5-a39f-f2aa84107c2e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KeyStop"",
                    ""type"": ""Button"",
                    ""id"": ""6bada699-0389-4db9-81c0-d0d0056235c7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f955a3ca-64b4-4d89-b94f-67bd96dbb1c3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7382b1d2-15f3-42fb-bd26-6ffbb1de2e25"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bfc6921b-5ff4-456b-823a-cae1ed6b9cfd"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseDrag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc88155c-16e3-4f19-b159-c64567ca605a"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CommandClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""008353a1-e2cf-460d-aeb6-1fa0cf0e9bdf"",
                    ""path"": ""<Keyboard>/#(A)"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14208069-4fb1-45bc-99c9-95e37c93f052"",
                    ""path"": ""<Keyboard>/#(S)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Mouse
        m_Mouse = asset.FindActionMap("Mouse", throwIfNotFound: true);
        m_Mouse_SelectClick = m_Mouse.FindAction("SelectClick", throwIfNotFound: true);
        m_Mouse_MousePosition = m_Mouse.FindAction("MousePosition", throwIfNotFound: true);
        m_Mouse_MouseDrag = m_Mouse.FindAction("MouseDrag", throwIfNotFound: true);
        m_Mouse_CommandClick = m_Mouse.FindAction("CommandClick", throwIfNotFound: true);
        m_Mouse_KeyAttack = m_Mouse.FindAction("KeyAttack", throwIfNotFound: true);
        m_Mouse_KeyStop = m_Mouse.FindAction("KeyStop", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Mouse
    private readonly InputActionMap m_Mouse;
    private IMouseActions m_MouseActionsCallbackInterface;
    private readonly InputAction m_Mouse_SelectClick;
    private readonly InputAction m_Mouse_MousePosition;
    private readonly InputAction m_Mouse_MouseDrag;
    private readonly InputAction m_Mouse_CommandClick;
    private readonly InputAction m_Mouse_KeyAttack;
    private readonly InputAction m_Mouse_KeyStop;
    public struct MouseActions
    {
        private @MouseInput m_Wrapper;
        public MouseActions(@MouseInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @SelectClick => m_Wrapper.m_Mouse_SelectClick;
        public InputAction @MousePosition => m_Wrapper.m_Mouse_MousePosition;
        public InputAction @MouseDrag => m_Wrapper.m_Mouse_MouseDrag;
        public InputAction @CommandClick => m_Wrapper.m_Mouse_CommandClick;
        public InputAction @KeyAttack => m_Wrapper.m_Mouse_KeyAttack;
        public InputAction @KeyStop => m_Wrapper.m_Mouse_KeyStop;
        public InputActionMap Get() { return m_Wrapper.m_Mouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseActions set) { return set.Get(); }
        public void SetCallbacks(IMouseActions instance)
        {
            if (m_Wrapper.m_MouseActionsCallbackInterface != null)
            {
                @SelectClick.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnSelectClick;
                @SelectClick.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnSelectClick;
                @SelectClick.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnSelectClick;
                @MousePosition.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnMousePosition;
                @MouseDrag.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnMouseDrag;
                @MouseDrag.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnMouseDrag;
                @MouseDrag.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnMouseDrag;
                @CommandClick.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnCommandClick;
                @CommandClick.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnCommandClick;
                @CommandClick.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnCommandClick;
                @KeyAttack.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnKeyAttack;
                @KeyAttack.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnKeyAttack;
                @KeyAttack.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnKeyAttack;
                @KeyStop.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnKeyStop;
                @KeyStop.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnKeyStop;
                @KeyStop.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnKeyStop;
            }
            m_Wrapper.m_MouseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SelectClick.started += instance.OnSelectClick;
                @SelectClick.performed += instance.OnSelectClick;
                @SelectClick.canceled += instance.OnSelectClick;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @MouseDrag.started += instance.OnMouseDrag;
                @MouseDrag.performed += instance.OnMouseDrag;
                @MouseDrag.canceled += instance.OnMouseDrag;
                @CommandClick.started += instance.OnCommandClick;
                @CommandClick.performed += instance.OnCommandClick;
                @CommandClick.canceled += instance.OnCommandClick;
                @KeyAttack.started += instance.OnKeyAttack;
                @KeyAttack.performed += instance.OnKeyAttack;
                @KeyAttack.canceled += instance.OnKeyAttack;
                @KeyStop.started += instance.OnKeyStop;
                @KeyStop.performed += instance.OnKeyStop;
                @KeyStop.canceled += instance.OnKeyStop;
            }
        }
    }
    public MouseActions @Mouse => new MouseActions(this);
    public interface IMouseActions
    {
        void OnSelectClick(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnMouseDrag(InputAction.CallbackContext context);
        void OnCommandClick(InputAction.CallbackContext context);
        void OnKeyAttack(InputAction.CallbackContext context);
        void OnKeyStop(InputAction.CallbackContext context);
    }
}
