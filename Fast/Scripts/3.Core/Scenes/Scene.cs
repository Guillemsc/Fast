using System;
using UnityEngine;

namespace Fast.Scenes
{
    [System.Serializable]
    public class Scene
    {
        [SerializeField] private string name = "";

        public Scene(string name)
        {
            this.name = name;
        }

        public string Name => name;
    }
}
