using System;
using UnityEngine;

namespace Fast.PrefabScenes
{
    public class PrefabScene<T> : BasePrefabScene where T : MonoBehaviour
    {
        public PrefabScene(Fast.Scenes.LoadedScene loaded_scene, T instance) : base(loaded_scene, instance)
        {

        }

        public T Instance => base.instance as T; 
    }
}
