using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Scenes
{ 
    public class LoadedScene
    {
        private readonly Scene scene = null;
        private readonly SceneRoot scene_root = null;
        private readonly UnityEngine.SceneManagement.Scene unity_scene = default;

        public LoadedScene(Scene scene, SceneRoot root, UnityEngine.SceneManagement.Scene unity_scene)
        {
            this.scene = scene;
            this.unity_scene = unity_scene;
        }

        public Scene Scene => scene;
        public SceneRoot SceneRoot => scene_root;
        public UnityEngine.SceneManagement.Scene UnityScene => unity_scene;
    }
}
