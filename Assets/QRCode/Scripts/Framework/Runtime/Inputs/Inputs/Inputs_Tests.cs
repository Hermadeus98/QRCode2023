//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/QRCode/Scripts/Framework/Runtime/Inputs/Inputs/Inputs_Tests.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Inputs_Tests: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Inputs_Tests()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Inputs_Tests"",
    ""maps"": [
        {
            ""name"": ""Tests"",
            ""id"": ""ae91eb92-99ac-4623-b508-7bd60c494102"",
            ""actions"": [
                {
                    ""name"": ""PrimaryAction"",
                    ""type"": ""Button"",
                    ""id"": ""0372df8d-bab7-4145-bbec-e8dbe2f61874"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SecondaryAction"",
                    ""type"": ""Button"",
                    ""id"": ""44844788-740c-4563-ba61-0de7f57d9ad2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""d1d23a5e-8ddd-4888-b96d-42d7b13b18b4"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MoveUp"",
                    ""type"": ""Button"",
                    ""id"": ""569639e3-c143-43a1-8675-e3576aaaf935"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TestInpuDisplayName"",
                    ""type"": ""Button"",
                    ""id"": ""7c29cdac-da29-4b36-ac61-57374532a6d3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""217cb623-0e4e-4276-9fc5-0ed982a0612b"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""274a7e06-8096-433e-bc6b-ec0d35540b4f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""f177d612-e395-4202-b9c5-1bb554cd73e5"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""XboxController"",
                    ""id"": ""93bdd45a-3197-4429-9095-c9b726b7fc34"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f2240f57-463d-463c-ae2b-a8eee0a5b1d2"",
                    ""path"": ""<XInputController>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxController"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""069620e7-0692-47cd-9285-dfc1462806af"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxController"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1c54fcac-26d1-4ddd-bb94-220b6f007034"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PrimaryAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a91d97e3-c174-40a3-abe5-8a5ab7c11e48"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxController"",
                    ""action"": ""PrimaryAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ace6390-7fd1-4afe-94f1-edd4d2e5e938"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxController"",
                    ""action"": ""SecondaryAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b294a2d-560a-4b65-85e9-f45462674c22"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SecondaryAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fec34a7d-9891-434c-9539-bb0c9b79239d"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d4d31a89-b502-4cf2-afdb-6281e637f154"",
                    ""path"": ""<XInputController>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxController"",
                    ""action"": ""MoveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""XboxController"",
            ""bindingGroup"": ""XboxController"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Tests
        m_Tests = asset.FindActionMap("Tests", throwIfNotFound: true);
        m_Tests_PrimaryAction = m_Tests.FindAction("PrimaryAction", throwIfNotFound: true);
        m_Tests_SecondaryAction = m_Tests.FindAction("SecondaryAction", throwIfNotFound: true);
        m_Tests_Move = m_Tests.FindAction("Move", throwIfNotFound: true);
        m_Tests_MoveUp = m_Tests.FindAction("MoveUp", throwIfNotFound: true);
        m_Tests_TestInpuDisplayName = m_Tests.FindAction("TestInpuDisplayName", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Tests
    private readonly InputActionMap m_Tests;
    private List<ITestsActions> m_TestsActionsCallbackInterfaces = new List<ITestsActions>();
    private readonly InputAction m_Tests_PrimaryAction;
    private readonly InputAction m_Tests_SecondaryAction;
    private readonly InputAction m_Tests_Move;
    private readonly InputAction m_Tests_MoveUp;
    private readonly InputAction m_Tests_TestInpuDisplayName;
    public struct TestsActions
    {
        private @Inputs_Tests m_Wrapper;
        public TestsActions(@Inputs_Tests wrapper) { m_Wrapper = wrapper; }
        public InputAction @PrimaryAction => m_Wrapper.m_Tests_PrimaryAction;
        public InputAction @SecondaryAction => m_Wrapper.m_Tests_SecondaryAction;
        public InputAction @Move => m_Wrapper.m_Tests_Move;
        public InputAction @MoveUp => m_Wrapper.m_Tests_MoveUp;
        public InputAction @TestInpuDisplayName => m_Wrapper.m_Tests_TestInpuDisplayName;
        public InputActionMap Get() { return m_Wrapper.m_Tests; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TestsActions set) { return set.Get(); }
        public void AddCallbacks(ITestsActions instance)
        {
            if (instance == null || m_Wrapper.m_TestsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_TestsActionsCallbackInterfaces.Add(instance);
            @PrimaryAction.started += instance.OnPrimaryAction;
            @PrimaryAction.performed += instance.OnPrimaryAction;
            @PrimaryAction.canceled += instance.OnPrimaryAction;
            @SecondaryAction.started += instance.OnSecondaryAction;
            @SecondaryAction.performed += instance.OnSecondaryAction;
            @SecondaryAction.canceled += instance.OnSecondaryAction;
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @MoveUp.started += instance.OnMoveUp;
            @MoveUp.performed += instance.OnMoveUp;
            @MoveUp.canceled += instance.OnMoveUp;
            @TestInpuDisplayName.started += instance.OnTestInpuDisplayName;
            @TestInpuDisplayName.performed += instance.OnTestInpuDisplayName;
            @TestInpuDisplayName.canceled += instance.OnTestInpuDisplayName;
        }

        private void UnregisterCallbacks(ITestsActions instance)
        {
            @PrimaryAction.started -= instance.OnPrimaryAction;
            @PrimaryAction.performed -= instance.OnPrimaryAction;
            @PrimaryAction.canceled -= instance.OnPrimaryAction;
            @SecondaryAction.started -= instance.OnSecondaryAction;
            @SecondaryAction.performed -= instance.OnSecondaryAction;
            @SecondaryAction.canceled -= instance.OnSecondaryAction;
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @MoveUp.started -= instance.OnMoveUp;
            @MoveUp.performed -= instance.OnMoveUp;
            @MoveUp.canceled -= instance.OnMoveUp;
            @TestInpuDisplayName.started -= instance.OnTestInpuDisplayName;
            @TestInpuDisplayName.performed -= instance.OnTestInpuDisplayName;
            @TestInpuDisplayName.canceled -= instance.OnTestInpuDisplayName;
        }

        public void RemoveCallbacks(ITestsActions instance)
        {
            if (m_Wrapper.m_TestsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ITestsActions instance)
        {
            foreach (var item in m_Wrapper.m_TestsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_TestsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public TestsActions @Tests => new TestsActions(this);
    private int m_XboxControllerSchemeIndex = -1;
    public InputControlScheme XboxControllerScheme
    {
        get
        {
            if (m_XboxControllerSchemeIndex == -1) m_XboxControllerSchemeIndex = asset.FindControlSchemeIndex("XboxController");
            return asset.controlSchemes[m_XboxControllerSchemeIndex];
        }
    }
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface ITestsActions
    {
        void OnPrimaryAction(InputAction.CallbackContext context);
        void OnSecondaryAction(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnMoveUp(InputAction.CallbackContext context);
        void OnTestInpuDisplayName(InputAction.CallbackContext context);
    }
}
