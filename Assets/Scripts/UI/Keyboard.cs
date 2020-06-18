using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    /// <summary>
    /// Static accessible instance of the Keyboard (Singleton pattern)
    /// </summary>
    public static Keyboard Instance { get; private set; }

    [SerializeField] 
    private TextMeshProUGUI _targetTextField;
    
    private float _keyboardXSize, _keyboardYSize;
    
    [SerializeField] 
    private float _buttonSize = 40, _margin = 10;

    [SerializeField]
    private bool[] _centerRows;
    
    private Transform[] _buttonRows;
    private float[] _rowWidths;

    private KeyboardButton[] _buttons;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        
        _buttonRows = new Transform[transform.childCount];
        for (int i = 0; i < _buttonRows.Length; i++)
        {
            _buttonRows[i] = transform.GetChild(i);
        }
        _rowWidths = new float[_buttonRows.Length];
    }

    private void Start()
    {
        _buttons = GetComponentsInChildren<KeyboardButton>();
        
        SetupButtons();
        OffsetRows();
        CenterRows();
        
        // Setup size of canvas
        GetComponent<RectTransform>().sizeDelta = new Vector2(_keyboardXSize, _keyboardYSize);
        GetComponent<BoxCollider>().size = new Vector3(_keyboardXSize, _keyboardYSize, 0);
        
        gameObject.SetActive(false);
    }

    private void SetupButtons()
    {
        int indexOffset = 0;
        for (int i = 0; i < _buttonRows.Length; i++)
        {
            int buttonAmountInRow = _buttonRows[i].childCount;

            _rowWidths[i] = 0;
            // Additional offset is used when a button uses the xSizeMultiplier
            // Then the next buttons have to be placed further on the x axis
            float additionalXOffset = 0;
            for (int j = 0; j < buttonAmountInRow; j++)
            {
                int buttonIndex = j + indexOffset;
                if (buttonIndex >= _buttons.Length)
                {
                    return;
                }
                
                _rowWidths[i] += SetupButton(_buttons[buttonIndex], j, ref additionalXOffset);
                _rowWidths[i] += _margin;
            }

            // Remove margin of last button from the calculated width
            _rowWidths[i] -= _margin;
            
            indexOffset += buttonAmountInRow;
        }
    }

    /// <summary>
    /// Sets up the size, position and collider of the button.
    /// Returns the final width of the button.
    /// </summary>
    /// <param name="button">Button to setup</param>
    /// <param name="rowIndex">Index of the button in its row</param>
    /// <param name="additionalXOffset">Additional offset on the x-axis, used when a button before was bigger</param>
    private float SetupButton(KeyboardButton button, int rowIndex, ref float additionalXOffset)
    {
        Vector2 origSize = new Vector2(_buttonSize, _buttonSize);
        
        button.SetFontSize(_buttonSize * 0.66f);

        RectTransform rectTransform = button.GetComponent<RectTransform>();
        Vector2 size = new Vector2(origSize.x * button.xSizeMultiplier, origSize.y);
        
        rectTransform.sizeDelta = size;
        rectTransform.anchoredPosition = new Vector2(rowIndex * (_buttonSize + _margin) + additionalXOffset, 0);
        additionalXOffset += size.x - origSize.x;

        BoxCollider col = button.GetComponent<BoxCollider>();
        col.center = new Vector3(size.x / 2, -size.y / 2, 0);
        col.size = new Vector3(size.x, size.y, 0);

        if (_targetTextField)
        {
            SetTargetTextfield(_targetTextField);
        }

        return size.x;
    }

    private void OffsetRows()
    {
        for (int i = 0; i < _buttonRows.Length; i++)
        {
            RectTransform row = _buttonRows[i].GetComponent<RectTransform>();

            float xOffset = _buttonSize * 0.5f * i;
            float yPos = -(_buttonSize + _margin) * i;

            // Do not offset if the row should be centered
            if (i < _centerRows.Length && _centerRows[i])
            {
                xOffset = 0;
            }
            
            _rowWidths[i] += xOffset;
            if (_rowWidths[i] > _keyboardXSize)
            {
                _keyboardXSize = _rowWidths[i];
            }
            
            row.anchoredPosition = new Vector2(xOffset, yPos);
        }

        _keyboardYSize = ((_buttonSize + _margin) * _buttonRows.Length) - _margin;
    }

    private void CenterRows()
    {
        for (int i = 0; i < _buttonRows.Length; i++)
        {
            if (i >= _centerRows.Length || !_centerRows[i])
            {
                continue;
            }
            
            float offset = (_keyboardXSize / 2f) - (_rowWidths[i] / 2);
            _buttonRows[i].GetComponent<RectTransform>().anchoredPosition += new Vector2(offset, 0);
        }
    }

    public void SetTargetTextfield(TextMeshProUGUI textfield)
    {
        _targetTextField = textfield;
        foreach (KeyboardButton button in _buttons)
        { 
            button.SetTargetTextfield(textfield);   
        }
    }
}
