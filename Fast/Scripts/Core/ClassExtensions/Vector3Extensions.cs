using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3Int ToVector3Int(this Vector3 vector)
    {
        return new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z);
    }

    public static Fast.Float3 ToFloat3(this Vector3 vector)
    {
        return new Fast.Float3(vector.x, vector.y, vector.z);
    }

    public static Fast.Int3 ToInt3(this Vector3 vector)
    {
        return new Fast.Int3((int)vector.x, (int)vector.y, (int)vector.z);
    }
}
