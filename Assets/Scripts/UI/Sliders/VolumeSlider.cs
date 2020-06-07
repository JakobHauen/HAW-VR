using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : UISlider
{
    private bool _isDragging;

    private Vector3 _sliderLeftEnd, _sliderRightEnd;

    protected override void Awake()
    {
        base.Awake();

        _slider.value = 0;
        _sliderLeftEnd = transform.position;

        _slider.value = 1;
        _sliderRightEnd = transform.position;
    }

    private void Start()
    {
        InputManager.Instance.OnLeftTriggerUp += OnTriggerUp;
    }

    public override void OnClick()
    {
        base.OnClick();

        if (!_isDragging)
        {
            StartCoroutine(C_Drag());
            _isDragging = true;
        }
    }

    private void OnTriggerUp()
    {
        _isDragging = false;
    }

    private IEnumerator C_Drag()
    {
        while (_isDragging)
        {
            yield return null;
        }

        StartCoroutine(C_ChangeBackColor());
    }
}
