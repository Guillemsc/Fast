using UnityEngine;

public static class Vector2Extensions
{
    public static Vector2Int ToVector2Int(this Vector2 vector)
    {
        return new Vector2Int((int)vector.x, (int)vector.y);
    }

    public static Fast.Float2 ToFloat2(this Vector2 vector)
    {
        return new Fast.Float2(vector.x, vector.y);
    }

    public static Fast.Int2 ToInt2(this Vector2 vector)
    {
        return new Fast.Int2((int)vector.x, (int)vector.y);
    }
}
