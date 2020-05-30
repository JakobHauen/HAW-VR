using System;
using System.Collections;
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
    private bool _hasLeftController, _hasRightController;

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
    }

    private void Start()
    {
        StartCoroutine(FetchControllerLeft());
        StartCoroutine(FetchControllerRight());
        InputDevices.deviceConnected += DeviceConnected;
        InputDevices.deviceDisconnected += DeviceDisconnected;
    }

    private IEnumerator FetchControllerLeft()
    {
        List<InputDevice> leftHandedControllers = new List<InputDevice>();
        InputDeviceCharacteristics characteristicsLeft = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left;
        InputDevices.GetDevicesWithCharacteristics(characteristicsLeft, leftHandedControllers);
        if (leftHandedControllers.Count > 0)
        {
            _leftController = leftHandedControllers[0];
            _hasLeftController = true;
            yield break;
        }

        // Keep searching for the controller if it has not been found yet
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FetchControllerLeft());
    }
    
    private IEnumerator FetchControllerRight()
    {
        List<InputDevice> rightHandedControllers = new List<InputDevice>();
        InputDeviceCharacteristics characteristicsRight = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right;
        InputDevices.GetDevicesWithCharacteristics(characteristicsRight, rightHandedControllers);
        if (rightHandedControllers.Count > 0)
        {
            _rightController = rightHandedControllers[0];
            _hasRightController = true;
            yield break;
        }
        
        // Keep searching for the controller if it has not been found yet
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FetchControllerRight());
    }
    
    private void DeviceConnected(InputDevice device)
    {
        UpdateLeftController(device);
        UpdateRightController(device);
    }
    
    private void UpdateLeftController(InputDevice device)
    {
        if (_hasLeftController || _leftController == device)
        {
            return;
        }
        
        InputDeviceCharacteristics desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left;
        if ((device.characteristics & desiredCharacteristics) == desiredCharacteristics)
        {
            _leftController = device;
            _hasLeftController = true;
        }
    }
    
    private void UpdateRightController(InputDevice device)
    {
        if (_hasRightController || _rightController == device)
        {
            return;
        }
        
        InputDeviceCharacteristics desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right;
        if ((device.characteristics & desiredCharacteristics) == desiredCharacteristics)
        {
            _rightController = device;
            _hasRightController = true;
        }
    }

    private void DeviceDisconnected(InputDevice device)
    {
        if (device == _leftController)
        {
            _hasLeftController = false;
        }
        else if (device == _rightController)
        {
            _hasRightController = false;
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

    private void LeftControllerUpdate()
    {
        if (!_hasLeftController)
        {
            return;
        }
        
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
    }

    private void RightControllerUpdate()
    {
        if (!_hasRightController)
        {
            return;
        }
        
        Vector2 stickAxis;
        bool buttonState;

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

    // Update is called once per frame
    private void Update()
    {
        LeftControllerUpdate();
        RightControllerUpdate();
    }
    
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
