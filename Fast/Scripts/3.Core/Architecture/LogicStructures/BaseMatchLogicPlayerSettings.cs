using System;

namespace Fast.Architecture
{
    public class BaseMatchLogicPlayerSettings
    {
        private MatchLogicPlayerNature player_nature = MatchLogicPlayerNature.USER;

        protected MatchLogicPlayerNature PlayerNature => player_nature;
    }
}
