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
        private readonly Fast.SceneServices.SceneService scene_service = null;
        private readonly UnityEngine.SceneManagement.Scene unity_scene = default;

        public LoadedScene(Scene scene, Fast.SceneServices.SceneService scene_service, UnityEngine.SceneManagement.Scene unity_scene)
        {
            this.scene = scene;
            this.scene_service = scene_service;
            this.unity_scene = unity_scene;
        }

        public Scene Scene => scene;
        public Fast.SceneServices.SceneService SceneService => scene_service;
        public UnityEngine.SceneManagement.Scene UnityScene => unity_scene;
    }
}
