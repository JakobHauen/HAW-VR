using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPointer : MonoBehaviour
{
    private UILineRenderer _uiLineRenderer;
    private UIInteractable _lastHitInteractable;

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

        Vector3 forward = InputManager.Instance.GetLeftControllerRotation() * Vector3.forward;
        Ray ray = new Ray(InputManager.Instance.GetLeftControllerPosition(), forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, layer))
        {
            _uiLineRenderer.SetTarget(hitInfo.point);

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
        }
        else if (_lastHitInteractable)
        {
            _lastHitInteractable.OnPointerLeave();
            _lastHitInteractable = null;
            _uiLineRenderer.ClearTarget();
        }
    }

    public void Enable()
    {
        InputManager.Instance.OnLeftTriggerDown += OnButtonClick;
        _uiLineRenderer.Enable();
        enabled = true;
    }

    public void Disable()
    {
        InputManager.Instance.OnLeftTriggerDown -= OnButtonClick;
        _uiLineRenderer.Disable();
        enabled = false;
    }

    private void OnButtonClick()
    {
        if (_lastHitInteractable && _lastHitInteractable.isEnabled)
        {
            _lastHitInteractable.OnClick();
        }
    }
}
