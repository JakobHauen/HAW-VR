using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
    /// <summary>
    /// Static accessible instance of the InputManager (Singleton pattern)
    /// </summary>
    public static InputManager Instance { get; private set; }
    
    private InputDevice _leftController, _rightController;

#region Input Values
    
    private Vector2 _leftControllerStickAxis  = Vector2.zero,
                    _rightControllerStickAxis = Vector2.zero;

    private bool _isLeftTriggerDown,
        _isLeftGripDown,
        _isMenuButtonDown,
        _isRightTriggerDown,
        _isRightGripDown;
    
#endregion
    
#region Event declarations
    
#region Left Controller
    
    public event Action<Vector2> OnLeftStickMove;
    public event Action OnLeftTriggerDown;    
    public event Action OnLeftTriggerUp;
    public event Action OnLeftGripDown;
    public event Action OnLeftGripUp;
    public event Action OnMenuButtonDown;
    public event Action OnMenuButtonUp;
    #endregion

#region Right Controller
    
    public event Action<Vector2> OnRightStickMove;
    public event Action OnRightTriggerDown;    
    public event Action OnRightTriggerUp;
    public event Action OnRightGripDown;
    public event Action OnRightGripUp;

#endregion
    
#endregion
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        
        // Always keep this object alive
        DontDestroyOnLoad(gameObject);

        FetchLeftController();
        FetchRightController();
    }

    private void FetchLeftController()
    {
        List<InputDevice> leftHandedControllers = new List<InputDevice>();
        InputDeviceCharacteristics desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);
        if (leftHandedControllers.Count != 1)
        {
            Debug.LogError("None or multiple left handed controllers found!");
            return;
        }

        _leftController = leftHandedControllers[0];
    }
    
    private void FetchRightController()
    {
        List<InputDevice> rightHandedControllers = new List<InputDevice>();
        InputDeviceCharacteristics desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, rightHandedControllers);
        if (rightHandedControllers.Count != 1)
        {
            Debug.LogError("None or multiple right handed controllers found!");
            return;
        }

        _rightController = rightHandedControllers[0];
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 stickAxis;
        bool buttonState;
        
        // LEFT STICK
        if (_leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out stickAxis))
        {
            if (_leftControllerStickAxis != stickAxis)
            {
                _leftControllerStickAxis = stickAxis;
                LeftStickMove();
            }
        }
        
        // LEFT TRIGGER
        if (_leftController.TryGetFeatureValue(CommonUsages.triggerButton, out buttonState))
        {
            if (_isLeftTriggerDown != buttonState)
            {
                _isLeftTriggerDown = buttonState;
                LeftTrigger();
            }
        }
        
        // LEFT GRIP
        if (_leftController.TryGetFeatureValue(CommonUsages.gripButton, out buttonState))
        {
            if (_isLeftGripDown != buttonState)
            {
                _isLeftGripDown = buttonState;
                LeftGrip();
            }
        }

        // MENU BUTTON (LEFT)
        if (_leftController.TryGetFeatureValue(CommonUsages.menuButton, out buttonState))
        {
            if (_isMenuButtonDown != buttonState)
            {
                _isMenuButtonDown = buttonState;
                MenuButton();
            }
        }
        
        // RIGHT STICK
        if (_rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out stickAxis))
        {
            if (_rightControllerStickAxis != stickAxis)
            {
                _rightControllerStickAxis = stickAxis;
                RightStickMove();
            }
        }
        
        // RIGHT TRIGGER
        if (_rightController.TryGetFeatureValue(CommonUsages.triggerButton, out buttonState))
        {
            if (_isRightTriggerDown != buttonState)
            {
                _isRightTriggerDown = buttonState;
                RightTrigger();
            }
        }
        
        // RIGHT GRIP
        if (_rightController.TryGetFeatureValue(CommonUsages.gripButton, out buttonState))
        {
            if (_isRightGripDown != buttonState)
            {
                _isRightGripDown = buttonState;
                RightGrip();
            }
        }
    }
    
#region Controller Orientation

    public Vector3 GetLeftControllerPosition()
    {
        Vector3 pos;
        return _leftController.TryGetFeatureValue(CommonUsages.devicePosition, out pos) ? pos + OVRManager.instance.transform.position : Vector3.zero;
    }

    public Quaternion GetLeftControllerRotation()
    {
        Quaternion rot;
        return _leftController.TryGetFeatureValue(CommonUsages.deviceRotation, out rot) ? rot : Quaternion.identity;
    }

    public Vector3 GetRightControllerPosition()
    {
        Vector3 pos;
        return _rightController.TryGetFeatureValue(CommonUsages.devicePosition, out pos) ? pos + OVRManager.instance.transform.position : Vector3.zero;
    }
    
    public Quaternion GetRightControllerRotation()
    {
        Quaternion rot;
        return _rightController.TryGetFeatureValue(CommonUsages.deviceRotation, out rot) ? rot : Quaternion.identity;
    }
    
#endregion
    
#region Event invoking
    
    private void LeftStickMove()
    {
        OnLeftStickMove?.Invoke(_leftControllerStickAxis);
    }

    private void LeftTrigger()
    {
        if (_isLeftTriggerDown)
        {
            OnLeftTriggerDown?.Invoke();
        }
        else
        {
            OnLeftTriggerUp?.Invoke();
        }
    }

    private void LeftGrip()
    {
        if (_isLeftGripDown)
        {
            OnLeftGripDown?.Invoke();
        }
        else
        {
            OnLeftGripUp?.Invoke();
        }
    }

    private void MenuButton()
    {
        if (_isMenuButtonDown)
        {
            OnMenuButtonDown?.Invoke();
        }
        else
        {
            OnMenuButtonUp?.Invoke();
        }
    }
    
    private void RightStickMove()
    {
        OnRightStickMove?.Invoke(_rightControllerStickAxis);
    }
    
    private void RightTrigger()
    {
        if (_isRightTriggerDown)
        {
            OnRightTriggerDown?.Invoke();
        }
        else
        {
            OnRightTriggerUp?.Invoke();
        }
    }

    private void RightGrip()
    {
        if (_isRightGripDown)
        {
            OnRightGripDown?.Invoke();
        }
        else
        {
            OnRightGripUp?.Invoke();
        }
    }
    
    #endregion
}
