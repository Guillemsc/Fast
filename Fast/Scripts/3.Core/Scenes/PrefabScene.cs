using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Scenes
{ 
    public class PrefabScene
    {
        private readonly Fast.Scenes.Scene scene = null;

        public PrefabScene(Fast.Scenes.Scene scene)
        {
            this.scene = scene;
        }

        public Fast.Scenes.Scene Scene => scene;

        public virtual void OnSceneLoaded(UnityEngine.SceneManagement.Scene loaded_unity_scene)
        {

        }
    }
}
