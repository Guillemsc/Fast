using System;
using System.Collections.Generic;

namespace Fast.Logic.Match
{
    public abstract class LogicMatchData
    {
        protected readonly LogicMatchSettings settings = null;
        protected readonly IReadOnlyList<LogicMatchPlayer> players_data = null;

        public LogicMatchData(LogicMatchSettings settings, IReadOnlyList<LogicMatchPlayer> players_data)
        {
            this.settings = settings;
            this.players_data = players_data;
        }
    }
}
