using System;
using System.Collections.Generic;

namespace Fast.Architecture
{
    public abstract class MatchLogicSettings
    {
        protected int game_mode_index = 0;

        private List<MatchLogicPlayerSettings> players_settings = new List<MatchLogicPlayerSettings>();

        protected IReadOnlyList<MatchLogicPlayerSettings> PlayersSettings => players_settings;
    }
}
