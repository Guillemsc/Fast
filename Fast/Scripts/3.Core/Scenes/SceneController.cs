using System;
using System.Collections.Generic;

namespace Fast.Scenes
{
    public class SceneController
    {
        private readonly List<Scene> scenes = new List<Scene>();
        private readonly List<LoadedScene> loaded_scenes = new List<LoadedScene>();
    }
}
