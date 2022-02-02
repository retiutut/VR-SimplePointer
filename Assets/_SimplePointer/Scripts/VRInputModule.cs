using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class VRInputModule : BaseInput
{

    public Camera eventCamera = null;
    public InputActionReference testReference = null;
    // public OVRInput.Controller controller = OVRInput.Controller.All;
    UnityEngine.XR.InputDevice controller;
    public bool printStuff = true;
    bool pressed = false;
    bool pressedDown = false;
    bool released = false;

    protected override void Awake()
    {
        GetComponent<BaseInputModule>().inputOverride = this;
        testReference.action.started += DoPressedThing;
        testReference.action.performed += DoChangeThing;
        testReference.action.canceled += DoReleasedThing;
    }

    private void DoPressedThing(InputAction.CallbackContext context)
    {
        if (printStuff)
            print("Pressed");
        pressed = true;
    }

    private void DoChangeThing(InputAction.CallbackContext context)
    {
        if (printStuff)
            print(context.ReadValue<float>());
        pressedDown = true;
    }

    private void DoReleasedThing(InputAction.CallbackContext context)
    {
        if (printStuff)
            print("Released");
        released = true;
    }

    public override bool GetMouseButton(int button)
    {
        print("OVERRIDE MOUSE BUTTON DOWN");
        return pressed;
    }

    public override bool GetMouseButtonDown(int button)
    {
        return pressedDown;
    }

    public override bool GetMouseButtonUp(int button)
    {
        return released;
    }

    public override Vector2 mousePosition
    {
        get
        {
            return new Vector2(eventCamera.pixelWidth / 2, eventCamera.pixelHeight / 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (controller == null)
        {
            var rightHandedControllers = new List<UnityEngine.XR.InputDevice>();
            var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, rightHandedControllers);
            foreach (var device in rightHandedControllers)
            {
                print(string.Format("Device name '{0}' has characteristics '{1}'", device.name, device.characteristics.ToString()));
                controller = device;
            }
        }
        //pressed = false;
        //pressedDown = false;
        //released = false;
    }
}
