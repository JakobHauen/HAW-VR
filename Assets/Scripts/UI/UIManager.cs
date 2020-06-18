using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(UIPointer))]
public class UIManager : MonoBehaviour
{
    // singleton pattern
    public static UIManager Instance { get; private set; }

    private Camera _cam;

    [SerializeField]
    private GameObject _menuCanvas, textFieldCanvas;

    private TextMeshProUGUI _textField;

    private bool _isShowingMenu;

    private UIPointer _uiPointer;
    private Keyboard _keyboard;
    
    public event Action<string> OnTextSubmit;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);

        _textField = textFieldCanvas.GetComponentInChildren<TextMeshProUGUI>();
        _uiPointer = GetComponent<UIPointer>();
    }

    private void Start()
    {
        _cam = Camera.main;
        _keyboard = Keyboard.Instance;

        _menuCanvas.SetActive(false);
        textFieldCanvas.SetActive(false);
        
        _uiPointer.Disable();
        InputManager.Instance.LeftController.OnMenuButtonDown += OnMenuButton;
    }

    private void OnMenuButton()
    {
        if (_isShowingMenu)
        {
            _menuCanvas.SetActive(false);
            _uiPointer.Disable();
        }
        else
        {

            PlaceUI(_menuCanvas.transform);
            _uiPointer.Enable();
        }
        _isShowingMenu = !_isShowingMenu;
    }

    public void ShowKeyboard(bool hideMenu = true)
    {
        _uiPointer.Enable();
        
        PlaceUI(_keyboard.transform);
        PlaceUI(textFieldCanvas.transform);

        if (hideMenu)
        {
            _menuCanvas.SetActive(false);
        }
    }

    public void HideKeyboard(bool disablePointer = true)
    {
        _keyboard.gameObject.SetActive(false);
        textFieldCanvas.SetActive(false);

        if (disablePointer)
        {
            _uiPointer.Disable();
        }
    }

    private void PlaceUI(Transform uiObject)
    {
        Vector3 pos = uiObject.position;
        Vector3 targetPosition = _cam.transform.position + (_cam.transform.forward * 3);
        targetPosition.y = pos.y;
        uiObject.position = targetPosition;

        Quaternion rot = Quaternion.LookRotation(_cam.transform.forward);
        uiObject.rotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);

        uiObject.gameObject.SetActive(true);
    }

    public void SubmitText()
    {
        OnTextSubmit?.Invoke(_textField.text);
        _textField.text = "";
    }
 }
