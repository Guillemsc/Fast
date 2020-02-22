using System;
using System.Collections.Generic;

namespace Fast.Logic
{
    public class MatchLogicSettings
    {
        private int game_mode_index = 0;

        private List<MatchLogicPlayerSettings> players_settings = new List<MatchLogicPlayerSettings>();

        protected List<MatchLogicPlayerSettings> PlayersSettings
        {
            get { return players_settings; }
        }
    }
}
