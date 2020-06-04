using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPointer : MonoBehaviour
{
    private UILineRenderer _uiLineRenderer;
    private UIButton _lastHitButton;
    private UISlider _lastHitSlider;

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

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, layer)) {
            UIButton targetButton = hitInfo.transform.GetComponent<UIButton>();

            if (targetButton && targetButton.isEnabled)
            {
                if (!_lastHitButton)
                {
                    _lastHitButton = targetButton;
                    _lastHitButton.OnPointerEnter();
                }
                else if (_lastHitButton && _lastHitButton != targetButton)
                {
                    _lastHitButton.OnPointerLeave();
                    _lastHitButton = targetButton;
                    _lastHitButton.OnPointerEnter();
                }

                _uiLineRenderer.SetTarget(hitInfo.point);
            }
            else
            {
                UISlider targetSlider = hitInfo.transform.GetComponent<UISlider>();
                if (targetSlider && targetSlider.isEnabled)
                {
                    if (!_lastHitSlider)
                    {
                        _lastHitSlider = targetSlider;
                        _lastHitSlider.OnPointerEnter();
                    }
                    else if (_lastHitSlider && _lastHitSlider != targetSlider)
                    {
                        _lastHitSlider.OnPointerLeave();
                        _lastHitSlider = targetSlider;
                        _lastHitSlider.OnPointerEnter();
                    }

                    _uiLineRenderer.SetTarget(hitInfo.point);
                }
            }
        }
        else
        {
            if (_lastHitButton)
            {
                _lastHitButton.OnPointerLeave();
                _lastHitButton = null;
                _uiLineRenderer.ClearTarget();
            }   
            else if (_lastHitSlider)
            {
                _lastHitSlider.OnPointerLeave();
                _lastHitSlider = null;
                _uiLineRenderer.ClearTarget();
            }
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
        if (_lastHitButton && _lastHitButton.isEnabled)
        {
            _lastHitButton.OnClick();
        }
        else if (_lastHitSlider && _lastHitSlider.isEnabled)
        {
            _lastHitSlider.OnClick();
        }
    }
}
