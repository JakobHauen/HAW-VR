using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ChatToggleSlider : UISlider
{
    protected override void Awake()
    {
        base.Awake();
        _slider.value = 1;
    }

    public override void OnClick(Vector3 hitPoint)
    {
        base.OnClick(hitPoint);

        _slider.value = (_slider.value == 0) ? 1 : 0;
    }

    public override void OnDrag(Vector3 hitPoint) { }
}