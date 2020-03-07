using System;
using System.Collections.Generic;

namespace Fast.Logic
{
    public class BaseMatchLogicData
    {
        private int game_mode_index = 0;

        private List<BaseMatchLogicSettings> players_settings = new List<BaseMatchLogicSettings>();

        private List<BaseMatchLogicPlayerData> players_data = new List<BaseMatchLogicPlayerData>();

        protected List<BaseMatchLogicSettings> PlayersSettings
        {
            get { return players_settings; }
        }

        protected List<BaseMatchLogicPlayerData> PlayersData
        {
            get { return players_data; }
        }
    }
}
