﻿using UnityEngine;

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

    public static float AngleBetweenPoints(this Vector2 vector, Vector2 to_check)
    {
        float xDiff = to_check.x - vector.x;
        float yDiff = to_check.y - vector.y;

        return Mathf.Atan2(yDiff, xDiff) * 180.0f / Mathf.PI;
    }

    public static Vector2 PerpendicularClockwise(this Vector2 vector)
    {
        return new Vector2(vector.y, -vector.x);
    }

    public static Vector2 PerpendicularCounterClockwise(this Vector2 vector)
    {
        return new Vector2(-vector.y, vector.x);
    }
}
