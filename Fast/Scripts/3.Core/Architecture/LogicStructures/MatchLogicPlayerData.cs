using System;

namespace Fast.Architecture
{
    public abstract class MatchLogicPlayerData
    {
        private readonly MatchLogicPlayerSettings settings = null;

        public MatchLogicPlayerData(MatchLogicPlayerSettings settings)
        {
            this.settings = settings;
        }
    }
}
