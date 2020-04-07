using System;
using UnityEngine;

namespace Fast.PrefabScenes
{
    public class BasePrefabScene
    {
        private readonly string uid = "";
        private readonly Fast.Scenes.LoadedScene loaded_scene = null;
        protected readonly MonoBehaviour instance = null;

        public BasePrefabScene(string uid, Fast.Scenes.LoadedScene loaded_scene, MonoBehaviour instance)
        {
            this.uid = uid;
            this.loaded_scene = loaded_scene;
            this.instance = instance;
        }

        public string UID => uid;
        public Fast.Scenes.LoadedScene LoadedScene => loaded_scene;
        public MonoBehaviour MonoBehaviourInstance => instance;
    }
}
