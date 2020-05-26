using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementTrajectory))]
public class MovementController : MonoBehaviour
{
    /// <summary>
    /// Static accessible instance of the MovementController (Singleton pattern)
    /// </summary>
    public static MovementController Instance { get; private set; }
    
    private MovementTrajectory _trajectory;
    private MovementCircle _circle;
    private Transform _circleTransform;
    private Vector3 _lastTargetPosition;

    [SerializeField] 
    private Color _indicatorColor;
    
    [Range(2, 64)]
    [SerializeField]
    private int _indicatorResolution = 20;

    // Remove _leftController when button manager exists
    [SerializeField]
    private Transform _playerController, _leftController;

    [SerializeField]
    private float _fadeDuration = 0.33f;
    
    [SerializeField]
    private OVRScreenFade _screenFade;

    public bool IsMoving { get; private set; }

    private UnityEngine.XR.InputDevice _device;

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
        
        IsMoving = false;
        _trajectory = GetComponent<MovementTrajectory>();
        _circle = GetComponentInChildren<MovementCircle>();
        
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private void Start()
    {
        _circleTransform = _circle.transform;
        _circle.SetColor(_indicatorColor);
        _circle.SetResolution(_indicatorResolution);
        _circle.enabled = false;
        
        _trajectory.SetColor(_indicatorColor);
        _trajectory.SetResolution(_indicatorResolution);
        
       FetchLeftController();
    }

    private void FetchLeftController()
    {
        List<UnityEngine.XR.InputDevice> leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDeviceCharacteristics desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);
        _device = leftHandedControllers[0];
    }
    
    // Remove when button manager exists
    private void Update()
    {
        if (_device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out Vector2 stickAxis))
        {
            if (stickAxis.y > 0.8f)
            {
                OnThumbstickPushUp(_leftController);
            }
            else
            {
                OnThumbstickRelease();
            }
        }
    }
    
    /// <summary>
    /// Shows the player the movement / teleport indicator.
    /// </summary>
    /// <param name="leftController"></param>
    public void OnThumbstickPushUp(Transform leftController)
    {
        PreviewMovement(leftController.position, leftController.rotation);
    }

    public void OnThumbstickRelease()
    {
        Move();
    }
    
    private void PreviewMovement(Vector3 raycastOrigin, Quaternion raycastRotation)
    {
        Ray ray = new Ray(raycastOrigin, raycastRotation * Vector3.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            _lastTargetPosition = hitInfo.point;
            
            _circle.EnableLineRenderer();
            _circleTransform.position = _lastTargetPosition;
            
            _trajectory.EnableLineRenderer();
            _trajectory.DrawTrajectory(raycastOrigin, raycastRotation, _lastTargetPosition);
        }
        else
        {
            _trajectory.DisableLineRenderer();
            _circle.DisableLineRenderer();
        }
    }

    private void Move()
    {
        if (IsMoving || _lastTargetPosition == Vector3.zero)
        {
            return;
        }
        
        _trajectory.DisableLineRenderer();
        _circle.DisableLineRenderer();
        StartCoroutine(C_Move());
    }
    
    /// <summary>
    /// Fades to black using the OVRScreenFade object, moves the player and fades back.
    /// </summary>
    /// <returns></returns>
    private IEnumerator C_Move()
    {
        IsMoving = true;

        float t = 0;
        while (t < 1)
        {
            _screenFade.SetFadeLevel(t);
            t += Time.deltaTime / _fadeDuration;
            yield return null;
        }
        _screenFade.SetFadeLevel(1);

        _playerController.position = _lastTargetPosition;
        _lastTargetPosition = Vector3.zero;
        
        t = 1;
        while (t > 0)
        {
            _screenFade.SetFadeLevel(t);
            t -= Time.deltaTime / _fadeDuration;
            yield return null;
        }
        _screenFade.SetFadeLevel(0);

        IsMoving = false;
    }
}
