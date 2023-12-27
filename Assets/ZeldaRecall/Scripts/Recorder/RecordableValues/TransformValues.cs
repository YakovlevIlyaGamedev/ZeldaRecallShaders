using UnityEngine;

public struct TransformValues
{
    public TransformValues(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    public Vector3 Position { get; }
    public Quaternion Rotation { get; }
}
