using UnityEngine;

public struct VelocityValues
{
    public VelocityValues(Vector3 velocity, Vector3 angularVelocity)
    {
        Velocity = velocity;
        AngularVelocity = angularVelocity;
    }

    public Vector3 Velocity { get; }
    public Vector3 AngularVelocity { get; }
}
