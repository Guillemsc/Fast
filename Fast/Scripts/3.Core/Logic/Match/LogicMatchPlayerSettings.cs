using System;

namespace Fast.Logic.Match
{
    public abstract class LogicMatchPlayerSettings
    {
        private readonly LogicMatchPlayerNature player_nature = LogicMatchPlayerNature.USER;

        public LogicMatchPlayerSettings(LogicMatchPlayerNature player_nature)
        {
            this.player_nature = player_nature;
        }

        protected LogicMatchPlayerNature PlayerNature => player_nature;
    }
}
