using System;

namespace Fast
{
    public interface IModule
    {
        void Awake();
        void Start();
        void CleanUp();
    }
}
