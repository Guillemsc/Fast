using System;

namespace Fast.Modules
{
    public interface IModule
    {
        void Awake();
        void Start();
        void CleanUp();
    }
}
