using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Scenes
{
    public class Scene
    {
        private readonly string name = "";

        public Scene(string name)
        {
            this.name = name;
        }

        public string Name => name;
    }
}
