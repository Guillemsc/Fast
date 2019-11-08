using UnityEngine;

public static class Vector2Extensions
{
    public static Vector2Int ToVector2Int(this Vector2 vector)
    {
        return new Vector2Int((int)vector.x, (int)vector.y);
    }
}
