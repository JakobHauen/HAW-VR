using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIPointer))]
public class UIManager : MonoBehaviour
{
    // singleton pattern
    public static UIManager Instance { get; private set; }

    private Camera _cam;

    [SerializeField]
    private GameObject _menuCanvas, textFieldCanvas;

    private bool _isShowingMenu;

    private UIPointer _uiPointer;
    private Keyboard _keyboard;

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

        _uiPointer = GetComponent<UIPointer>();
    }

    private void Start()
    {
        _cam = Camera.main;
        _keyboard = Keyboard.Instance;

        _menuCanvas.SetActive(false);
        _uiPointer.Disable();
        InputManager.Instance.LeftController.OnMenuButtonDown += OnMenuButton;

        ShowKeyboard();
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

    public void ShowKeyboard()
    {
        PlaceUI(_keyboard.transform);
        PlaceUI(textFieldCanvas.transform);

        _menuCanvas.SetActive(false);
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
 }
