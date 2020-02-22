using System;
using System.Collections.Generic;

namespace Fast.Logic
{
    public class MatchLogicData
    {
        private int game_mode_index = 0;

        private List<MatchLogicSettings> players_settings = new List<MatchLogicSettings>();

        private List<MatchLogicPlayerData> players_data = new List<MatchLogicPlayerData>();

        protected List<MatchLogicSettings> PlayersSettings
        {
            get { return players_settings; }
        }

        protected List<MatchLogicPlayerData> PlayersData
        {
            get { return players_data; }
        }
    }
}
