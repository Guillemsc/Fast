using System;

namespace Fast.Architecture
{
    public abstract class MatchLogicPlayerSettings
    {
        private readonly MatchLogicPlayerNature player_nature = MatchLogicPlayerNature.USER;

        public MatchLogicPlayerSettings(MatchLogicPlayerNature player_nature)
        {
            this.player_nature = player_nature;
        }

        protected MatchLogicPlayerNature PlayerNature => player_nature;
    }
}
