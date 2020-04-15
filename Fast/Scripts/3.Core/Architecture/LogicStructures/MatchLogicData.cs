using System;
using System.Collections.Generic;

namespace Fast.Architecture
{
    public abstract class MatchLogicData
    {
        private readonly MatchLogicSettings settings = null;
        private readonly IReadOnlyList<MatchLogicPlayerData> players_data = null;

        public MatchLogicData(MatchLogicSettings settings, IReadOnlyList<MatchLogicPlayerData> players_data)
        {
            this.settings = settings;
            this.players_data = players_data;
        }
    }
}
