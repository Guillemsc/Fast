using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Scenes
{
    public class SceneResolver
    {
        private readonly List<Scene> scenes_to_resolve = new List<Scene>();

        public IReadOnlyList<Scene> ScenesToResolve => scenes_to_resolve;

        public void AddSceneToResolve(Scene scene)
        {
            for(int i = 0; i < scenes_to_resolve.Count; ++i)
            {
                if(scenes_to_resolve[i] == scene)
                {
                    return;
                }
            }

            scenes_to_resolve.Add(scene);
        }
    }
}
