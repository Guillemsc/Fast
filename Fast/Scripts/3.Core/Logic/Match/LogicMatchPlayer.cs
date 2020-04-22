using System;

namespace Fast.Logic.Match
{
    public abstract class LogicMatchPlayer
    {
        private readonly LogicMatchPlayerSettings settings = null;

        public LogicMatchPlayer(LogicMatchPlayerSettings settings)
        {
            this.settings = settings;
        }
    }
}
