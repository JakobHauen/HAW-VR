using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPointer : MonoBehaviour
{
    private UILineRenderer _uiLineRenderer;

    private UIButton _lastHitButton;

    private void Awake()
    {
        _uiLineRenderer = GetComponentInChildren<UILineRenderer>();
        InputManager.Instance.OnLeftTriggerDown += OnButtonClick;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    private void Update()
    {
        int layer = LayerMask.NameToLayer("UI");

        Vector3 forward = InputManager.Instance.GetLeftControllerRotation() * Vector3.forward;
        Ray ray = new Ray(InputManager.Instance.GetLeftControllerPosition(), forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, layer)) {
            UIButton target = hitInfo.transform.GetComponent<UIButton>();
            if (!_lastHitButton)
            {
                _lastHitButton = target;
                _lastHitButton.OnPointerEnter();
            }
            else if (_lastHitButton && _lastHitButton != target)
            {
                _lastHitButton.OnPointerLeave();
                _lastHitButton = target;
                _lastHitButton.OnPointerEnter();
            }

            _uiLineRenderer.SetTarget(hitInfo.point);
        }
        else if (_lastHitButton)
        {
            _lastHitButton.OnPointerLeave();
            _lastHitButton = null;
            _uiLineRenderer.ClearTarget();
        }
    }

    private void OnButtonClick()
    {
        if (_lastHitButton)
        {
            _lastHitButton.OnClick();
        }
    }
}
