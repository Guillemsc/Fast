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

        public LoadedScene(Scene scene, SceneRoot scene_root)
        {
            this.scene = scene;
            this.scene_root = scene_root;
        }

        public Scene Scene => scene;
        public SceneRoot SceneRoot => scene_root;
    }
}
