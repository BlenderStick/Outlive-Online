// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/MouseInput.inputactions'

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
                    ""name"": ""SelectDown"",
                    ""type"": ""Button"",
                    ""id"": ""67bcbef5-7780-4e2f-b698-a543b2d21bf9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""SelectUp"",
                    ""type"": ""Button"",
                    ""id"": ""8cdddefd-26e5-4519-9b1a-52ad5a2600d6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
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
                    ""name"": ""CommandClick"",
                    ""type"": ""Button"",
                    ""id"": ""494f2e6e-040d-4b8d-bb84-553ce99cc861"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Multiselect"",
                    ""type"": ""Button"",
                    ""id"": ""a4b52898-88fe-461a-9008-94ee40cd4634"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CancelSelect"",
                    ""type"": ""Button"",
                    ""id"": ""1a36902b-3271-47c8-b8ea-e2b10d2f5214"",
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
                    ""action"": ""SelectDown"",
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
                    ""id"": ""933e1c09-a640-4f8e-8796-046a7f4266eb"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Multiselect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37509b37-f51a-46d0-bb8e-c1625ea88422"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CancelSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c5c1822a-d9c3-466d-a47e-9c28d57f8a2e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CancelSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45d1eb67-78af-4109-bebb-d55974e9a04f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Shortcut"",
            ""id"": ""5102c3a5-b584-482b-b119-f2c665c9bb5f"",
            ""actions"": [
                {
                    ""name"": ""ActionClick"",
                    ""type"": ""Button"",
                    ""id"": ""21303b18-83fb-4ff7-b1c7-3460988db355"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ActionRelease"",
                    ""type"": ""Button"",
                    ""id"": ""2206f709-6ead-48b8-8e3c-9602ff4027e0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""CancelClick"",
                    ""type"": ""Button"",
                    ""id"": ""1bca4965-284f-4cd2-b85d-a35d6addd781"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6f7b7815-3ab4-4fce-9606-7ead598b69dc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KeyQ"",
                    ""type"": ""Button"",
                    ""id"": ""4161f413-1b81-4579-b4d0-f16a199b6924"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""KeyW"",
                    ""type"": ""Button"",
                    ""id"": ""6dc20490-0eda-4583-9f7f-7296dd0b88d5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KeyE"",
                    ""type"": ""Button"",
                    ""id"": ""92720ff9-bdd9-4022-b056-34e46406e8ea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KeyR"",
                    ""type"": ""Button"",
                    ""id"": ""31ad1e05-a99d-4c05-a47e-3768158686c5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KeyA"",
                    ""type"": ""Button"",
                    ""id"": ""2ce0c0c9-8b7b-4336-9935-eb412825ff19"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KeyS"",
                    ""type"": ""Button"",
                    ""id"": ""fd60c39e-2923-4724-b03d-81510ce5f8bf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KeyD"",
                    ""type"": ""Button"",
                    ""id"": ""eee58596-9743-4c88-92ed-1c03c2758101"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KeyF"",
                    ""type"": ""Button"",
                    ""id"": ""f43080c1-afaa-4406-af0c-97d92d615f0c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KeyZ"",
                    ""type"": ""Button"",
                    ""id"": ""be53bb58-66f7-4359-8850-8a60f6bad904"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KeyX"",
                    ""type"": ""Button"",
                    ""id"": ""8ebf990c-6a44-4844-bd76-90157e1556a6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KeyC"",
                    ""type"": ""Button"",
                    ""id"": ""6133500f-2c78-487d-a392-143d2ba046be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KeyV"",
                    ""type"": ""Button"",
                    ""id"": ""47c5e9f1-b269-4b58-9a03-7c049657f71f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fd42886c-10d1-41b1-9a31-47fdb87fa811"",
                    ""path"": ""<Keyboard>/#(A)"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyA"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e9cafab-4795-4265-912f-18cfb76a492c"",
                    ""path"": ""<Keyboard>/#(S)"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyS"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""427b8fc6-a7b9-4dcf-b92d-aa2cf9a9e1de"",
                    ""path"": ""<Keyboard>/#(Q)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyQ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0906eca8-93e4-488b-bfdb-4baea060f6ed"",
                    ""path"": ""<Keyboard>/#(W)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyW"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""27aa63ce-2109-4a31-b1d7-4f5be1e2eb12"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ActionClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""76337a6b-4b90-4aa2-b1b5-dd214e9d5372"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CancelClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""26fab7a3-30f6-4c98-9902-8de4357c7339"",
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
                    ""id"": ""8affa3ac-4ea4-4e0a-8228-f0ef4efdf786"",
                    ""path"": ""<Keyboard>/#(F)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyF"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8105e5ce-0bcd-4397-b442-a389629bb014"",
                    ""path"": ""<Keyboard>/#(D)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca4f040b-d2b9-468d-a9e9-998891072981"",
                    ""path"": ""<Keyboard>/#(R)"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf761d34-d72a-4c4e-8735-a8a319124e7b"",
                    ""path"": ""<Keyboard>/#(E)"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyE"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""561dd0b7-5f97-40f9-93f4-c05cab93d665"",
                    ""path"": ""<Keyboard>/#(Z)"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyZ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38938a0f-5bcd-494e-8583-cb1503b05752"",
                    ""path"": ""<Keyboard>/#(X)"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56e3fae2-2db1-4ef9-9718-84fe3766df85"",
                    ""path"": ""<Keyboard>/#(C)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyC"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e609f949-e330-47b4-bfee-dd389340bf17"",
                    ""path"": ""<Keyboard>/#(V)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyV"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a9ecc9c-3f84-48a1-bb4c-7859f6f93dff"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ActionRelease"",
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
        m_Mouse_SelectDown = m_Mouse.FindAction("SelectDown", throwIfNotFound: true);
        m_Mouse_SelectUp = m_Mouse.FindAction("SelectUp", throwIfNotFound: true);
        m_Mouse_MousePosition = m_Mouse.FindAction("MousePosition", throwIfNotFound: true);
        m_Mouse_CommandClick = m_Mouse.FindAction("CommandClick", throwIfNotFound: true);
        m_Mouse_Multiselect = m_Mouse.FindAction("Multiselect", throwIfNotFound: true);
        m_Mouse_CancelSelect = m_Mouse.FindAction("CancelSelect", throwIfNotFound: true);
        // Shortcut
        m_Shortcut = asset.FindActionMap("Shortcut", throwIfNotFound: true);
        m_Shortcut_ActionClick = m_Shortcut.FindAction("ActionClick", throwIfNotFound: true);
        m_Shortcut_ActionRelease = m_Shortcut.FindAction("ActionRelease", throwIfNotFound: true);
        m_Shortcut_CancelClick = m_Shortcut.FindAction("CancelClick", throwIfNotFound: true);
        m_Shortcut_MousePosition = m_Shortcut.FindAction("MousePosition", throwIfNotFound: true);
        m_Shortcut_KeyQ = m_Shortcut.FindAction("KeyQ", throwIfNotFound: true);
        m_Shortcut_KeyW = m_Shortcut.FindAction("KeyW", throwIfNotFound: true);
        m_Shortcut_KeyE = m_Shortcut.FindAction("KeyE", throwIfNotFound: true);
        m_Shortcut_KeyR = m_Shortcut.FindAction("KeyR", throwIfNotFound: true);
        m_Shortcut_KeyA = m_Shortcut.FindAction("KeyA", throwIfNotFound: true);
        m_Shortcut_KeyS = m_Shortcut.FindAction("KeyS", throwIfNotFound: true);
        m_Shortcut_KeyD = m_Shortcut.FindAction("KeyD", throwIfNotFound: true);
        m_Shortcut_KeyF = m_Shortcut.FindAction("KeyF", throwIfNotFound: true);
        m_Shortcut_KeyZ = m_Shortcut.FindAction("KeyZ", throwIfNotFound: true);
        m_Shortcut_KeyX = m_Shortcut.FindAction("KeyX", throwIfNotFound: true);
        m_Shortcut_KeyC = m_Shortcut.FindAction("KeyC", throwIfNotFound: true);
        m_Shortcut_KeyV = m_Shortcut.FindAction("KeyV", throwIfNotFound: true);
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
    private readonly InputAction m_Mouse_SelectDown;
    private readonly InputAction m_Mouse_SelectUp;
    private readonly InputAction m_Mouse_MousePosition;
    private readonly InputAction m_Mouse_CommandClick;
    private readonly InputAction m_Mouse_Multiselect;
    private readonly InputAction m_Mouse_CancelSelect;
    public struct MouseActions
    {
        private @MouseInput m_Wrapper;
        public MouseActions(@MouseInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @SelectDown => m_Wrapper.m_Mouse_SelectDown;
        public InputAction @SelectUp => m_Wrapper.m_Mouse_SelectUp;
        public InputAction @MousePosition => m_Wrapper.m_Mouse_MousePosition;
        public InputAction @CommandClick => m_Wrapper.m_Mouse_CommandClick;
        public InputAction @Multiselect => m_Wrapper.m_Mouse_Multiselect;
        public InputAction @CancelSelect => m_Wrapper.m_Mouse_CancelSelect;
        public InputActionMap Get() { return m_Wrapper.m_Mouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseActions set) { return set.Get(); }
        public void SetCallbacks(IMouseActions instance)
        {
            if (m_Wrapper.m_MouseActionsCallbackInterface != null)
            {
                @SelectDown.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnSelectDown;
                @SelectDown.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnSelectDown;
                @SelectDown.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnSelectDown;
                @SelectUp.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnSelectUp;
                @SelectUp.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnSelectUp;
                @SelectUp.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnSelectUp;
                @MousePosition.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnMousePosition;
                @CommandClick.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnCommandClick;
                @CommandClick.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnCommandClick;
                @CommandClick.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnCommandClick;
                @Multiselect.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnMultiselect;
                @Multiselect.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnMultiselect;
                @Multiselect.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnMultiselect;
                @CancelSelect.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnCancelSelect;
                @CancelSelect.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnCancelSelect;
                @CancelSelect.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnCancelSelect;
            }
            m_Wrapper.m_MouseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SelectDown.started += instance.OnSelectDown;
                @SelectDown.performed += instance.OnSelectDown;
                @SelectDown.canceled += instance.OnSelectDown;
                @SelectUp.started += instance.OnSelectUp;
                @SelectUp.performed += instance.OnSelectUp;
                @SelectUp.canceled += instance.OnSelectUp;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @CommandClick.started += instance.OnCommandClick;
                @CommandClick.performed += instance.OnCommandClick;
                @CommandClick.canceled += instance.OnCommandClick;
                @Multiselect.started += instance.OnMultiselect;
                @Multiselect.performed += instance.OnMultiselect;
                @Multiselect.canceled += instance.OnMultiselect;
                @CancelSelect.started += instance.OnCancelSelect;
                @CancelSelect.performed += instance.OnCancelSelect;
                @CancelSelect.canceled += instance.OnCancelSelect;
            }
        }
    }
    public MouseActions @Mouse => new MouseActions(this);

    // Shortcut
    private readonly InputActionMap m_Shortcut;
    private IShortcutActions m_ShortcutActionsCallbackInterface;
    private readonly InputAction m_Shortcut_ActionClick;
    private readonly InputAction m_Shortcut_ActionRelease;
    private readonly InputAction m_Shortcut_CancelClick;
    private readonly InputAction m_Shortcut_MousePosition;
    private readonly InputAction m_Shortcut_KeyQ;
    private readonly InputAction m_Shortcut_KeyW;
    private readonly InputAction m_Shortcut_KeyE;
    private readonly InputAction m_Shortcut_KeyR;
    private readonly InputAction m_Shortcut_KeyA;
    private readonly InputAction m_Shortcut_KeyS;
    private readonly InputAction m_Shortcut_KeyD;
    private readonly InputAction m_Shortcut_KeyF;
    private readonly InputAction m_Shortcut_KeyZ;
    private readonly InputAction m_Shortcut_KeyX;
    private readonly InputAction m_Shortcut_KeyC;
    private readonly InputAction m_Shortcut_KeyV;
    public struct ShortcutActions
    {
        private @MouseInput m_Wrapper;
        public ShortcutActions(@MouseInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @ActionClick => m_Wrapper.m_Shortcut_ActionClick;
        public InputAction @ActionRelease => m_Wrapper.m_Shortcut_ActionRelease;
        public InputAction @CancelClick => m_Wrapper.m_Shortcut_CancelClick;
        public InputAction @MousePosition => m_Wrapper.m_Shortcut_MousePosition;
        public InputAction @KeyQ => m_Wrapper.m_Shortcut_KeyQ;
        public InputAction @KeyW => m_Wrapper.m_Shortcut_KeyW;
        public InputAction @KeyE => m_Wrapper.m_Shortcut_KeyE;
        public InputAction @KeyR => m_Wrapper.m_Shortcut_KeyR;
        public InputAction @KeyA => m_Wrapper.m_Shortcut_KeyA;
        public InputAction @KeyS => m_Wrapper.m_Shortcut_KeyS;
        public InputAction @KeyD => m_Wrapper.m_Shortcut_KeyD;
        public InputAction @KeyF => m_Wrapper.m_Shortcut_KeyF;
        public InputAction @KeyZ => m_Wrapper.m_Shortcut_KeyZ;
        public InputAction @KeyX => m_Wrapper.m_Shortcut_KeyX;
        public InputAction @KeyC => m_Wrapper.m_Shortcut_KeyC;
        public InputAction @KeyV => m_Wrapper.m_Shortcut_KeyV;
        public InputActionMap Get() { return m_Wrapper.m_Shortcut; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ShortcutActions set) { return set.Get(); }
        public void SetCallbacks(IShortcutActions instance)
        {
            if (m_Wrapper.m_ShortcutActionsCallbackInterface != null)
            {
                @ActionClick.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnActionClick;
                @ActionClick.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnActionClick;
                @ActionClick.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnActionClick;
                @ActionRelease.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnActionRelease;
                @ActionRelease.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnActionRelease;
                @ActionRelease.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnActionRelease;
                @CancelClick.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnCancelClick;
                @CancelClick.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnCancelClick;
                @CancelClick.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnCancelClick;
                @MousePosition.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnMousePosition;
                @KeyQ.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyQ;
                @KeyQ.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyQ;
                @KeyQ.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyQ;
                @KeyW.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyW;
                @KeyW.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyW;
                @KeyW.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyW;
                @KeyE.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyE;
                @KeyE.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyE;
                @KeyE.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyE;
                @KeyR.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyR;
                @KeyR.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyR;
                @KeyR.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyR;
                @KeyA.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyA;
                @KeyA.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyA;
                @KeyA.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyA;
                @KeyS.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyS;
                @KeyS.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyS;
                @KeyS.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyS;
                @KeyD.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyD;
                @KeyD.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyD;
                @KeyD.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyD;
                @KeyF.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyF;
                @KeyF.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyF;
                @KeyF.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyF;
                @KeyZ.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyZ;
                @KeyZ.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyZ;
                @KeyZ.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyZ;
                @KeyX.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyX;
                @KeyX.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyX;
                @KeyX.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyX;
                @KeyC.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyC;
                @KeyC.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyC;
                @KeyC.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyC;
                @KeyV.started -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyV;
                @KeyV.performed -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyV;
                @KeyV.canceled -= m_Wrapper.m_ShortcutActionsCallbackInterface.OnKeyV;
            }
            m_Wrapper.m_ShortcutActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ActionClick.started += instance.OnActionClick;
                @ActionClick.performed += instance.OnActionClick;
                @ActionClick.canceled += instance.OnActionClick;
                @ActionRelease.started += instance.OnActionRelease;
                @ActionRelease.performed += instance.OnActionRelease;
                @ActionRelease.canceled += instance.OnActionRelease;
                @CancelClick.started += instance.OnCancelClick;
                @CancelClick.performed += instance.OnCancelClick;
                @CancelClick.canceled += instance.OnCancelClick;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @KeyQ.started += instance.OnKeyQ;
                @KeyQ.performed += instance.OnKeyQ;
                @KeyQ.canceled += instance.OnKeyQ;
                @KeyW.started += instance.OnKeyW;
                @KeyW.performed += instance.OnKeyW;
                @KeyW.canceled += instance.OnKeyW;
                @KeyE.started += instance.OnKeyE;
                @KeyE.performed += instance.OnKeyE;
                @KeyE.canceled += instance.OnKeyE;
                @KeyR.started += instance.OnKeyR;
                @KeyR.performed += instance.OnKeyR;
                @KeyR.canceled += instance.OnKeyR;
                @KeyA.started += instance.OnKeyA;
                @KeyA.performed += instance.OnKeyA;
                @KeyA.canceled += instance.OnKeyA;
                @KeyS.started += instance.OnKeyS;
                @KeyS.performed += instance.OnKeyS;
                @KeyS.canceled += instance.OnKeyS;
                @KeyD.started += instance.OnKeyD;
                @KeyD.performed += instance.OnKeyD;
                @KeyD.canceled += instance.OnKeyD;
                @KeyF.started += instance.OnKeyF;
                @KeyF.performed += instance.OnKeyF;
                @KeyF.canceled += instance.OnKeyF;
                @KeyZ.started += instance.OnKeyZ;
                @KeyZ.performed += instance.OnKeyZ;
                @KeyZ.canceled += instance.OnKeyZ;
                @KeyX.started += instance.OnKeyX;
                @KeyX.performed += instance.OnKeyX;
                @KeyX.canceled += instance.OnKeyX;
                @KeyC.started += instance.OnKeyC;
                @KeyC.performed += instance.OnKeyC;
                @KeyC.canceled += instance.OnKeyC;
                @KeyV.started += instance.OnKeyV;
                @KeyV.performed += instance.OnKeyV;
                @KeyV.canceled += instance.OnKeyV;
            }
        }
    }
    public ShortcutActions @Shortcut => new ShortcutActions(this);
    public interface IMouseActions
    {
        void OnSelectDown(InputAction.CallbackContext context);
        void OnSelectUp(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnCommandClick(InputAction.CallbackContext context);
        void OnMultiselect(InputAction.CallbackContext context);
        void OnCancelSelect(InputAction.CallbackContext context);
    }
    public interface IShortcutActions
    {
        void OnActionClick(InputAction.CallbackContext context);
        void OnActionRelease(InputAction.CallbackContext context);
        void OnCancelClick(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnKeyQ(InputAction.CallbackContext context);
        void OnKeyW(InputAction.CallbackContext context);
        void OnKeyE(InputAction.CallbackContext context);
        void OnKeyR(InputAction.CallbackContext context);
        void OnKeyA(InputAction.CallbackContext context);
        void OnKeyS(InputAction.CallbackContext context);
        void OnKeyD(InputAction.CallbackContext context);
        void OnKeyF(InputAction.CallbackContext context);
        void OnKeyZ(InputAction.CallbackContext context);
        void OnKeyX(InputAction.CallbackContext context);
        void OnKeyC(InputAction.CallbackContext context);
        void OnKeyV(InputAction.CallbackContext context);
    }
}
