using UnityEngine;

public class MovementIndicator : MonoBehaviour
{
    protected LineRenderer LineRenderer { get; private set; }
    protected int Resolution { get; private set; }
    
    protected virtual void Awake()
    {
        LineRenderer = GetComponent<LineRenderer>();
    }

    public virtual void SetResolution(int resolution)
    {
        Resolution = resolution;
        LineRenderer.positionCount = Resolution;
    }
    
    public void SetColor(Color c)
    {
        Gradient colorGradient = LineRenderer.colorGradient;
        GradientColorKey[] colorKeys = colorGradient.colorKeys;
        GradientAlphaKey[] alphaKeys = colorGradient.alphaKeys;

        for (int i = 0; i < colorKeys.Length; i++)
        {
            colorKeys[i].color = c;
        }
        
        colorGradient.SetKeys(colorKeys, alphaKeys);
        LineRenderer.colorGradient = colorGradient;
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
