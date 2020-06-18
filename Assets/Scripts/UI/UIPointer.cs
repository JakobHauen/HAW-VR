using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPointer : MonoBehaviour
{
    private UILineRenderer _uiLineRenderer;
    private UIInteractable _lastHitInteractable;
    private Vector3 _lastHitPoint;

    private void Awake()
    {
        _uiLineRenderer = GetComponentInChildren<UILineRenderer>();

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    private void Update()
    {
        int layer = LayerMask.GetMask("UI");

        Vector3 forward = InputManager.Instance.CurrentlyUsedController.Rotation * Vector3.forward;
        Ray ray = new Ray(InputManager.Instance.CurrentlyUsedController.Position, forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, layer))
        {
            _lastHitPoint = hitInfo.point;
            _uiLineRenderer.SetTarget(_lastHitPoint);

            UIInteractable target = hitInfo.transform.GetComponent<UIInteractable>();
            if (target && target.isEnabled)
            {
                if (!_lastHitInteractable)
                {
                    _lastHitInteractable = target;
                    _lastHitInteractable.OnPointerEnter();
                }
                else if (_lastHitInteractable && _lastHitInteractable != target)
                {
                    _lastHitInteractable.OnPointerLeave();
                    _lastHitInteractable = target;
                    _lastHitInteractable.OnPointerEnter();
                }
            }
            else
            {
                ResetLastHit();
            }
        }
        else if (_lastHitInteractable)
        {
            ResetLastHit();
            _uiLineRenderer.ClearTarget();
        }
    }

    private void ResetLastHit()
    {
        if (_lastHitInteractable)
        {
            _lastHitInteractable.OnPointerLeave();
            _lastHitInteractable = null;
        }
    }

    public void Enable()
    {
        InputManager.Instance.CurrentlyUsedController.OnTriggerDown += OnButtonClick;
        InputManager.Instance.CurrentlyUsedController.OnTrigger += OnButton;
        InputManager.Instance.CurrentlyUsedController.OnTriggerUp += OnButtonUp;
        _uiLineRenderer.Enable();
        enabled = true;
    }

    public void Disable()
    {
        InputManager.Instance.CurrentlyUsedController.OnTriggerDown -= OnButtonClick;
        InputManager.Instance.CurrentlyUsedController.OnTrigger -= OnButton;
        InputManager.Instance.CurrentlyUsedController.OnTriggerUp -= OnButtonUp;
        _uiLineRenderer.Disable();
        enabled = false;
    }

    private void OnButtonClick()
    {
        if (_lastHitInteractable && _lastHitInteractable.isEnabled)
        {
            _lastHitInteractable.OnClick(_lastHitPoint);
        }
    }

    private void OnButton()
    {
        if (_lastHitInteractable && _lastHitInteractable.isEnabled)
        {
            if (_lastHitInteractable is UISlider s)
            {
                s.OnDrag(_lastHitPoint);
            }
        }
    }

    private void OnButtonUp()
    {
        if (_lastHitInteractable && _lastHitInteractable.isEnabled)
        {
            _lastHitInteractable.OnClickUp();
        }
    }
}
