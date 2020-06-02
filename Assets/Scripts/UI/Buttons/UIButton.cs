using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIButton : MonoBehaviour
{
    private Image _image;

    [SerializeField]
    private Color _normalColor = Color.white,
        _hoverColor = Color.white,
        _pressedColor = Color.white,
        _disabledColor = Color.white;

    [SerializeField]
    private bool _isEnabled = true;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.color = _isEnabled ? _normalColor : _disabledColor;
    }

    public void SetState(bool state)
    {
        _isEnabled = state;
        _image.color = state ? _normalColor : _disabledColor;
    }

    public virtual void OnPointerEnter()
    {
        if (!_isEnabled)
        {
            return;
        }

        _image.color = _hoverColor;
    }

    public virtual void OnPointerLeave()
    {
        if (!_isEnabled)
        {
            return;
        }

        _image.color = _normalColor;
    }

    public virtual void OnClick()
    {
        if (!_isEnabled)
        {
            return;
        }

        _image.color = _pressedColor;

        StartCoroutine(C_BackToNormalColor());
    }

    IEnumerator C_BackToNormalColor()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        _image.color = _normalColor;
    }
}