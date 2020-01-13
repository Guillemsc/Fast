using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public class MatchmakingCluster
    {
        private MatchmakingGameModeSettings game_mode_settings = null;

        private List<MatchmakingParty> matchmaking_parties = new List<MatchmakingParty>();

        private int total_players = 0;

        private bool complete = false;

        public MatchmakingCluster(MatchmakingGameModeSettings game_mode_settings)
        {
            this.game_mode_settings = game_mode_settings;
        }

        public MatchmakingGameModeSettings MatchmakingGameModeSettings
        {
            get { return game_mode_settings; }
        }

        public int TotalPlayers
        {
            get { return total_players; }
        }

        public void AddParties(List<MatchmakingParty> parties)
        {
            for(int i = 0; i < parties.Count; ++i)
            {
                AddParty(parties[i]);
            }
        }

        public void AddParty(MatchmakingParty party)
        {
            total_players += party.PlayersCount;

            matchmaking_parties.Add(party);
        }

        public void RemoveParty(MatchmakingParty party)
        {
            for (int i = 0; i < matchmaking_parties.Count; ++i)
            {
                MatchmakingParty curr_party = matchmaking_parties[i];

                if (curr_party == party)
                {
                    total_players -= party.PlayersCount;

                    matchmaking_parties.RemoveAt(i);

                    break;
                }
            }
        }

        public List<MatchmakingParty> Parties
        {
            get { return matchmaking_parties; }
        }
    }
}
