using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public class MatchmakingParty
    {
        private MatchmakingPlayer party_owner = null;

        private MatchmakingGameModeSettings game_mode_settings = null;

        private List<MatchmakingPlayer> matchmaking_players = new List<MatchmakingPlayer>();

        private int party_elo = 0;

        public MatchmakingParty(MatchmakingPlayer party_owner, MatchmakingGameModeSettings game_mode_settings)
        {
            this.party_owner = party_owner;
            this.game_mode_settings = game_mode_settings;
        }

        public MatchmakingPlayer PartyOwner
        {
            get { return party_owner; }
        }

        public MatchmakingGameModeSettings MatchmakingGameModeSettings
        {
            get { return game_mode_settings; }
        }

        public void AddPlayer(MatchmakingPlayer player)
        {
            matchmaking_players.Add(player);

            int total_elo = 0;

            for(int i = 0; i < matchmaking_players.Count; ++i)
            {
                MatchmakingPlayer curr_player = matchmaking_players[i];

                total_elo += curr_player.Elo;
            }

            party_elo = total_elo / matchmaking_players.Count;
        }

        public List<MatchmakingPlayer> MatchmakingPlayers
        {
            get { return matchmaking_players; }
        }

        public int PlayersCount
        {
            get { return matchmaking_players.Count; }
        }

        public int PartyElo
        {
            get { return party_elo; }
        }
    }
}
