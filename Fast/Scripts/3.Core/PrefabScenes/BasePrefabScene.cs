using System;
using UnityEngine;

namespace Fast.PrefabScenes
{
    public class BasePrefabScene
    {
        private readonly Fast.Scenes.LoadedScene loaded_scene = null;
        protected readonly MonoBehaviour instance = null;

        public BasePrefabScene(Fast.Scenes.LoadedScene loaded_scene, MonoBehaviour instance)
        {
            this.loaded_scene = loaded_scene;
            this.instance = instance;
        }

        public Fast.Scenes.LoadedScene LoadedScene => loaded_scene;
        public MonoBehaviour MonoBehaviourInstance => instance;
    }
}
