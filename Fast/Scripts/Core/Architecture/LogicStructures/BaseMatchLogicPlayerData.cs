﻿using System;

namespace Fast.Logic
{
    public class MatchLogicPlayerData
    {
        private MatchLogicPlayerNature player_nature = MatchLogicPlayerNature.CLIENT;

        protected MatchLogicPlayerNature PlayerNature
        {
            get { return player_nature; }
        }
    }
}