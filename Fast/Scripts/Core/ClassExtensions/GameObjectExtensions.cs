using UnityEngine;

public static class GameObjectExtensions
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T ret = go.GetComponent<T>();

        if(ret == null)
        {
            ret = go.AddComponent<T>();
        }

        return ret;
    }
}
