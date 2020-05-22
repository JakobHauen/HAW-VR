using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MovementCircle : MovementIndicator
{
    [SerializeField]
    private float _radius = 0.1f;

    protected override void Awake()
    {
        base.Awake();
        DisableLineRenderer();
    }

    public override void SetResolution(int resolution)
    {
        base.SetResolution(resolution);
        for (int i = 0; i < Resolution; i++)
        {
            float t = (float) i / Resolution;
            float rad = Mathf.Deg2Rad * t * 360;
            LineRenderer.SetPosition(i, new Vector3(Mathf.Sin(rad) * _radius, LineRenderer.widthMultiplier, Mathf.Cos(rad) * _radius));
        }
    }
}
