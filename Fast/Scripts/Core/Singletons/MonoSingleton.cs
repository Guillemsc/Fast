using UnityEngine;

namespace Fast
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;

        protected void InitInstance(T _instance)
        {
            if (_instance != null)
                instance = _instance;
        }

        public static T Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                else
                {
                    Debug.LogError("[Fast.MonoSingleton] Used before initialization. Please use InitInstance(T _instance) before" +
                        "using the singleton instance");

                    return null;
                }
            }
        }
    }
}
