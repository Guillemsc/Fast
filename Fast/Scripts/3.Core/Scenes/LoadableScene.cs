using System;

namespace Fast.Scenes
{
    public class LoadableScene
    {
        private string name = "";

        public string Name => name;

        public LoadableScene(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
