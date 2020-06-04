﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public abstract class UIButton : MonoBehaviour
{
    private RectTransform _rectTransform;

    private Image _image;

    private BoxCollider _collider;

    [SerializeField]
    private Color _normalColor = Color.white,
        _hoverColor = Color.white,
        _pressedColor = Color.white,
        _disabledColor = new Color(0.5f, 0.5f, 0.5f);

    public bool isEnabled = true;

    protected virtual void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        _image = GetComponent<Image>();
        _image.color = isEnabled ? _normalColor : _disabledColor;
    }

    public void SetState(bool state)
    {
        isEnabled = state;
        _image.color = state ? _normalColor : _disabledColor;
    }

    public virtual void OnPointerEnter()
    {
        _image.color = _hoverColor;
    }

    public virtual void OnPointerLeave()
    {
        _image.color = _normalColor;
    }

    public virtual void OnClick()
    {
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