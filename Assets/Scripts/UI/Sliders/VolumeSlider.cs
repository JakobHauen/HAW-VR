using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : UISlider
{
    private bool _isDragging;

    private float _sliderLeftX, _sliderRightX, _sliderStartEndDistance;

    protected override void Awake()
    {
        base.Awake();

        _slider.value = 0;
        Vector3 sliderLeftEnd = transform.InverseTransformPoint(_colorFeedbackImage.transform.position);
        _sliderLeftX = sliderLeftEnd.x;

        _slider.value = 1;
        Vector3 sliderRightEnd = transform.InverseTransformPoint(_colorFeedbackImage.transform.position);
        _sliderRightX = sliderRightEnd.x;

        _sliderStartEndDistance = _sliderRightX - _sliderLeftX;
    }

    /*
    private void Start()
    {
        InputManager.Instance.OnLeftTriggerUp += OnTriggerUp;
    }
    */

    public override void OnClick(Vector3 hitPoint)
    {
        base.OnClick(hitPoint);

        Vector3 localHitPoint = transform.InverseTransformPoint(hitPoint);
        float hitX = localHitPoint.x;
        float hitDistance = hitX - _sliderLeftX;
        float pos = hitDistance / _sliderStartEndDistance;
        _slider.value = pos;

        AudioListener.volume = pos;

        /*
        if (!_isDragging)
        {
            StartCoroutine(C_Drag());
            _isDragging = true;
        }
        */
    }

    /*
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
    */
}
