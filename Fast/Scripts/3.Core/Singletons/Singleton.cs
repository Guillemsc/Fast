using System;

namespace Fast
{
    public class Singleton<T> where T : class, new()
    {
        private static Lazy<T> instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Lazy<T>(() => new T());
                }

                return instance.Value;
            }
        }
    }
}
