using System;
using System.Collections.Generic;

namespace Fast.Architecture
{
    public class BaseMatchLogicSettings
    {
        protected int game_mode_index = 0;

        private List<BaseMatchLogicPlayerSettings> players_settings = new List<BaseMatchLogicPlayerSettings>();

        protected IReadOnlyList<BaseMatchLogicPlayerSettings> PlayersSettings => players_settings;
    }
}
