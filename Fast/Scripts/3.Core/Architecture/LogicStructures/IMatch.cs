using System;

namespace Fast.Architecture
{
    public interface IMatch 
    {
        void Init(MatchLogicSettings settings);
        void Start();
    }
}
