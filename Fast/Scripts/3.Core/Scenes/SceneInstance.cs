using System;
using UnityEngine;

namespace Fast.Scenes
{
    public abstract class SceneInstance : MonoBehaviour
    {
        private UnityEngine.SceneManagement.Scene unity_scene = default;

        public UnityEngine.SceneManagement.Scene UnityScene => unity_scene;

        public SceneInstance()
        {

        }

        public void Init(UnityEngine.SceneManagement.Scene unity_scene)
        {
            this.unity_scene = unity_scene;
        }
        
        public virtual void OnSceneLoaded()
        {

        }
    }
}
