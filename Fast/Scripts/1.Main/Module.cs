using System;

namespace Fast.Modules
{
    public class Module : IModule
    {
        private readonly FastService fast = null;

        public Module(FastService fast)
        {
            this.fast = fast;
        }

        public virtual void Awake()
        {

        }

        public virtual void Start()
        {

        }

        public virtual void CleanUp()
        {

        }

        protected FastService FastService => fast;
    }
}
