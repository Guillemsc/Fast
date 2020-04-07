using System;

namespace Fast.Scenes
{ 
    public class LoadedScene
    {
        private readonly Fast.Scenes.Scene scene = null;
        private readonly Fast.Scenes.SceneRoot scene_root = null;
        private readonly UnityEngine.SceneManagement.Scene unity_scene = default;

        public LoadedScene(Fast.Scenes.Scene scene, Fast.Scenes.SceneRoot scene_root, 
            UnityEngine.SceneManagement.Scene unity_scene)
        {
            this.scene = scene;
            this.scene_root = scene_root;
            this.unity_scene = unity_scene;
        }

        public Fast.Scenes.Scene Scene => scene;
        public Fast.Scenes.SceneRoot SceneRoot => scene_root;
        public UnityEngine.SceneManagement.Scene UnityScene => unity_scene;
    }
}
