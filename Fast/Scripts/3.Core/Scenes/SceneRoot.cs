using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Scenes
{
    public class SceneRoot 
    {
        private readonly List<GameObject> root_gameobjects = new List<GameObject>();

        public SceneRoot(IReadOnlyList<GameObject> root_gameobjects)
        {
            this.root_gameobjects.AddRange(root_gameobjects);
        }

        public IReadOnlyList<GameObject> RootGameObjects => root_gameobjects;
    }
}
