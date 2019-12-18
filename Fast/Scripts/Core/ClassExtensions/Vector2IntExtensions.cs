using UnityEngine;

public static class Vector2IntExtensions
{
    public static Vector2 ToVector2(this Vector2Int vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Fast.Float2 ToFloat2(this Vector2Int vector)
    {
        return new Fast.Float2(vector.x, vector.y);
    }

    public static Fast.Int2 ToInt2(this Vector2Int vector)
    {
        return new Fast.Int2(vector.x, vector.y);
    }
}
