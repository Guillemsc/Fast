using System;
using System.Collections.Generic;

namespace Fast.Networking
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MatchmakingGameModeSettings : Attribute
    {
        private string game_mode_id = "";
        private int timeout_seconds = 0;
        private int minimum_players = 0;
        private int maximum_players = 0;

        public MatchmakingGameModeSettings(string game_mode_id, int timeout_seconds, int minimum_players, int maximum_players)
        {
            this.game_mode_id = game_mode_id;
            this.timeout_seconds = timeout_seconds;
            this.minimum_players = minimum_players;
            this.maximum_players = maximum_players;
        }

        public string GameModeId
        {
            get { return game_mode_id; }
        }

        public int TimeoutSeconds
        {
            get { return timeout_seconds; }
        }

        public int MinimumPlayers
        {
            get { return minimum_players; }
        }

        public int MaximumPlayers
        {
            get { return maximum_players; }
        }
    }
}
