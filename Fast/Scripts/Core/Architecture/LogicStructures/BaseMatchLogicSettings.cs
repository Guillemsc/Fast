using System;
using System.Collections.Generic;

namespace Fast.Logic
{
    public class BaseMatchLogicSettings
    {
        private int game_mode_index = 0;

        private List<BaseMatchLogicPlayerSettings> players_settings = new List<BaseMatchLogicPlayerSettings>();

        protected List<BaseMatchLogicPlayerSettings> PlayersSettings
        {
            get { return players_settings; }
        }
    }
}
