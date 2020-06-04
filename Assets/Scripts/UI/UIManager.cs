using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIPointer))]
public class UIManager : MonoBehaviour
{
    // singleton pattern
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private GameObject _menuCanvas;

    private bool _isShowingMenu;

    private UIPointer _uiPointer;

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
        _menuCanvas.SetActive(false);
        _uiPointer.Disable();
        InputManager.Instance.OnMenuButtonDown += OnMenuButton;
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
            _menuCanvas.SetActive(true);
            _uiPointer.Enable();
        }
        _isShowingMenu = !_isShowingMenu;
    }
}
