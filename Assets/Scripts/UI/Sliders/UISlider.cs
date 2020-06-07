using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public abstract class UISlider : UIInteractable
{
    [SerializeField]
    protected Slider _slider;

    [SerializeField]
    private Image _handle;

    protected virtual void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        if (!_slider)
        {
            Slider s = GetComponent<Slider>();
            if (s)
            {
                _slider = s;
            }
        }
        _handle.color = isEnabled ? _normalColor : _disabledColor;
    }

    public void SetState(bool state)
    {
        isEnabled = state;
        _handle.color = state ? _normalColor : _disabledColor;
    }

    public override void OnPointerEnter()
    {
        _handle.color = _hoverColor;
        _isHovered = true;
    }

    public override void OnPointerLeave()
    {
        _handle.color = _normalColor;
        _isHovered = false;
    }

    public override void OnClick()
    {
        _handle.color = _pressedColor;
    }

    protected override IEnumerator C_ChangeBackColor()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        _handle.color = _isHovered ? _hoverColor : _normalColor;
    }
}