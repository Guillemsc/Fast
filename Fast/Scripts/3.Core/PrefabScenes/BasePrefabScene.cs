using System;
using UnityEngine;

namespace Fast.PrefabScenes
{
    public class BasePrefabScene
    {
        private readonly string name = "";
        protected readonly MonoBehaviour instance = null;

        public BasePrefabScene(string name, MonoBehaviour instance)
        {
            this.name = name;
            this.instance = instance;
        }

        public string Name => name;
        public MonoBehaviour MonoBehaviourInstance => instance;
    }
}
