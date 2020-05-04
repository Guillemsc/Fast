using System;

namespace Fast.Scenes
{
    public class SceneReference
    {
        private string name = "";

        private SceneInstance instance = null;

        public string Name => name;

        public SceneReference(string name)
        {
            this.name = name;
        }

        public void SetInstanceData(SceneInstance instance)
        {
            this.instance = instance;
        }

        public bool HasInstance()
        {
            return instance != null;
        }

        public bool HasInstance<T>() where T: SceneInstance
        {
            return (instance as T) != null;
        }

        public SceneInstance GetInstance()
        {
            return instance;
        }

        public T GetInstance<T>() where T : SceneInstance
        {
            return instance as T;
        }
    }
}
