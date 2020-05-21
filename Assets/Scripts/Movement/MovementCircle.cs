using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MovementCircle : MovementIndicator
{
    [SerializeField]
    private float _radius = 0.1f;

    private void Awake()
    {
        for (int i = 0; i < Resolution; i++)
        {
            float t = (float) i / Resolution;
            float rad = Mathf.Deg2Rad * t * 360;
            LineRenderer.SetPosition(i, new Vector3(Mathf.Sin(rad) * _radius, LineRenderer.widthMultiplier, Mathf.Cos(rad) * _radius));
        }

        DisableLineRenderer();
    }
}
