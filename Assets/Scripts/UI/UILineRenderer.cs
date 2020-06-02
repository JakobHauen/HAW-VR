using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class UILineRenderer : MonoBehaviour
{
    private LineRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = InputManager.Instance.GetLeftControllerPosition();
        transform.rotation = InputManager.Instance.GetLeftControllerRotation();
    }

    public void SetTarget(Vector3 targetWorldPos)
    {
        Vector3 targetLocalPos = transform.InverseTransformPoint(targetWorldPos);
        _renderer.SetPosition(1, targetLocalPos);
    }

    public void ClearTarget()
    {
        _renderer.SetPosition(1, Vector3.forward * 3);
    }
}
