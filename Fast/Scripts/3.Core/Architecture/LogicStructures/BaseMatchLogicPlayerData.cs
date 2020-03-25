using System;

namespace Fast.Architecture
{
    public class BaseMatchLogicPlayerData
    {
        private MatchLogicPlayerNature player_nature = MatchLogicPlayerNature.USER;

        protected MatchLogicPlayerNature PlayerNature => player_nature;
    }
}
