using System;
using UnityEngine;

namespace Fast.PrefabScenes
{
    public class PrefabSceneReference<T> where T : MonoBehaviour
    {
        private readonly string uid = "";
        private readonly string scene_name = "";

        public PrefabSceneReference(string uid, string scene_name)
        {
            this.uid = uid;
            this.scene_name = scene_name;
        }

        public string UID => uid;
        public string SceneName => scene_name;
    }
}
