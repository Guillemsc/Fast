using UnityEngine;

public static class Vector3IntExtensions
{
    public static Vector3 ToVector2(this Vector3Int vector)
    {
        return new Vector3(vector.x, vector.y, vector.z);
    }

    public static Vector2Int ToVector2Int(this Vector3Int vector)
    {
        return new Vector2Int(vector.x, vector.y);
    }

    public static Fast.Float3 ToFloat3(this Vector3Int vector)
    {
        return new Fast.Float3(vector.x, vector.y, vector.z);
    }

    public static Fast.Int3 ToInt3(this Vector3Int vector)
    {
        return new Fast.Int3(vector.x, vector.y, vector.z);
    }
}
