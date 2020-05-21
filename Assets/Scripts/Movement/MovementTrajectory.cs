using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MovementTrajectory : MovementIndicator
{
    private float _forwardVelocity, _upwardVelocity, _gravity;

    public void DrawTrajectory(Vector3 origin, Quaternion rotation, Vector3 target)
    {
        Vector3 forward = target - origin;
        Vector3 flatForward = Quaternion.Euler(0, rotation.eulerAngles.y, 0) * Vector3.forward;
        float dot = Mathf.Clamp(Vector3.Dot(flatForward, forward), 0, 10);

        _forwardVelocity = Vector3.Distance(origin, target);
        _upwardVelocity = dot;
        _gravity = _upwardVelocity * 2;
        for (int i = 0; i < Resolution; i++)
        {
            float t = (float) i / (Resolution - 1);
            Vector3 position = rotation * new Vector3(0, Y(t), X(t));
            position += origin;
            LineRenderer.SetPosition(i, position);
        }
    }

    private float X(float t)
    {
        return _forwardVelocity * t;
    }

    private float Y(float t)
    {
        return _upwardVelocity * t - (_gravity / 2) * (t * t);
    }
}
