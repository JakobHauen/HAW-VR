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

    public override void OnClick()
    {
        base.OnClick();

        if (_slider.value == 0)
        {
            _slider.value = 1;
        }
        else
        {
            _slider.value = 0;
        }

        StartCoroutine(C_BackToNormalColor());
    }
}
