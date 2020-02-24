using UnityEngine;

public static class TransformExtensions
{
    public static void SetPosition(this Transform transform, Vector3 set)
    {
        transform.position = set;
    }

    public static void SetPosition(this Transform transform, Vector2 set)
    {
        Vector3 new_position = transform.position;

        new_position.x = set.x;
        new_position.y = set.y;

        transform.position = new_position;
    }

    public static void AddToPosition(this Transform transform, Vector2 set)
    {
        Vector3 new_position = transform.position;

        new_position.x += set.x;
        new_position.y += set.y;

        transform.position = new_position;
    }

    public static void SetPositionX(this Transform transform, float set)
    {
        Vector3 new_position = transform.position;

        new_position.x = set;

        transform.position = new_position;
    }

    public static void SetPositionY(this Transform transform, float set)
    {
        Vector3 new_position = transform.position;

        new_position.y = set;

        transform.position = new_position;
    }

    public static void SetPositionZ(this Transform transform, float set)
    {
        Vector3 new_position = transform.position;

        new_position.z = set;

        transform.position = new_position;
    }

    public static void SetLocalPosition(this Transform transform, Vector2 set)
    {
        Vector3 new_position = transform.localPosition;

        new_position.x = set.x;
        new_position.y = set.y;

        transform.localPosition = new_position;
    }

    public static void AddToLocalPosition(this Transform transform, Vector2 set)
    {
        Vector3 new_position = transform.localPosition;

        new_position.x += set.x;
        new_position.y += set.y;

        transform.localPosition = new_position;
    }

    public static void SetLocalPositionX(this Transform transform, float set)
    {
        Vector3 new_position = transform.localPosition;

        new_position.x = set;

        transform.localPosition = new_position;
    }

    public static void SetLocalPositionY(this Transform transform, float set)
    {
        Vector3 new_position = transform.localPosition;

        new_position.y = set;

        transform.localPosition = new_position;
    }

    public static void SetLocalPositionZ(this Transform transform, float set)
    {
        Vector3 new_position = transform.localPosition;

        new_position.z = set;

        transform.localPosition = new_position;
    }

    public static void SetRotationEuler(this Transform transform, Vector3 set)
    {
        transform.rotation = Quaternion.Euler(set);
    }

    public static void SetLocalRotationEuler(this Transform transform, Vector3 set)
    {
        transform.localRotation = Quaternion.Euler(set);
    }
}
