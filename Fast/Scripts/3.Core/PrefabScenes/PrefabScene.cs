using System;
using UnityEngine;

namespace Fast.PrefabScenes
{
    public class PrefabScene<T> : BasePrefabScene where T : MonoBehaviour
    {
        public PrefabScene(string name, T instance) : base(name, instance)
        {

        }

        public T Instance => base.instance as T; 
    }
}
