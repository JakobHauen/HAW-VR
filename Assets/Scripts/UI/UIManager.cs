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
        _cam = Camera.main;

        _menuCanvas.SetActive(false);
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
            Vector3 pos = _menuCanvas.transform.position;
            Vector3 targetPosition = _cam.transform.position + (_cam.transform.forward * 2);
            targetPosition.y = pos.y;
            _menuCanvas.transform.position = pos;

            Quaternion rot = Quaternion.LookRotation(-_cam.transform.forward);
            _menuCanvas.transform.rotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);

            _menuCanvas.SetActive(true);
            _uiPointer.Enable();
        }
        _isShowingMenu = !_isShowingMenu;
    }
}
