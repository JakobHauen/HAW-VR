using UnityEngine;

public class MovementIndicator : MonoBehaviour
{
    protected LineRenderer LineRenderer { get; private set; }
    protected int Resolution { get; private set; }
    
    private void Awake()
    {
        LineRenderer = GetComponent<LineRenderer>();
    }

    public void SetResolution(int resolution)
    {
        Resolution = resolution;
        LineRenderer.positionCount = Resolution;
    }
    
    public void SetColor(Color c)
    {
        GradientColorKey[] colors = LineRenderer.colorGradient.colorKeys;
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i].color = c;
        }
        LineRenderer.colorGradient.colorKeys = colors;
    }

    public void EnableLineRenderer()
    {
        if (!LineRenderer.enabled)
        {
            LineRenderer.enabled = true;
        }
    }
    
    public void DisableLineRenderer()
    {
        if (LineRenderer.enabled)
        {
            LineRenderer.enabled = false;
        }
    }
}
