using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public abstract class UISlider : MonoBehaviour
{
    private RectTransform _rectTransform;

    [SerializeField]
    protected Slider _slider;

    [SerializeField]
    private Image _handle;

    private BoxCollider _collider;

    [SerializeField]
    private Color _normalColor = Color.white,
        _hoverColor = Color.white,
        _pressedColor = Color.white,
        _disabledColor = Color.white;

    public bool isEnabled = true;

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

    public virtual void OnPointerEnter()
    {
        _handle.color = _hoverColor;
    }

    public virtual void OnPointerLeave()
    {
        _handle.color = _normalColor;
    }

    public virtual void OnClick()
    {
        _handle.color = _pressedColor;
    }

    protected IEnumerator C_BackToNormalColor()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        _handle.color = _normalColor;
    }
}