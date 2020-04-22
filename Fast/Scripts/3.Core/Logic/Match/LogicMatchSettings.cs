using System;
using System.Collections.Generic;

namespace Fast.Logic.Match
{
    public abstract class LogicMatchSettings
    {
        protected int game_mode_index = 0;

        private List<LogicMatchPlayerSettings> players_settings = new List<LogicMatchPlayerSettings>();

        protected IReadOnlyList<LogicMatchPlayerSettings> PlayersSettings => players_settings;
    }
}
