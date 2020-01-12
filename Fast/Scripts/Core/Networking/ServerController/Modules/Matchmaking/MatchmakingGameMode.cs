using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public class MatchmakingGameMode
    {
        private MatchmakingGameModeSettings game_mode_settings = null;

        private List<MatchmakingCluster> matchmaking_clusters = new List<MatchmakingCluster>();

        public MatchmakingGameMode(MatchmakingGameModeSettings game_mode_settings)
        {
            this.game_mode_settings = game_mode_settings;
        }

        public MatchmakingGameModeSettings MatchmakingGameModeSettings
        {
            get { return game_mode_settings; }
        }

        public void AddCluster(MatchmakingCluster cluster)
        {
            matchmaking_clusters.Add(cluster);
        }

        public void RemoveCluster(MatchmakingCluster cluster)
        {
            for(int i = 0; i < matchmaking_clusters.Count; ++i)
            {
                MatchmakingCluster curr_cluster = matchmaking_clusters[i];

                if(curr_cluster == cluster)
                {
                    matchmaking_clusters.RemoveAt(i);

                    break;
                }
            }
        }

        public void MergeClusters(MatchmakingCluster c1, MatchmakingCluster to_merge)
        {
            for (int i = 0; i < matchmaking_clusters.Count; ++i)
            {
                MatchmakingCluster curr_cluster = matchmaking_clusters[i];

                if (to_merge == curr_cluster)
                {
                    c1.AddParties(to_merge.Parties);

                    matchmaking_clusters.RemoveAt(i);

                    break;
                }
            }
        }

        public List<MatchmakingCluster> Clusters
        {
            get { return matchmaking_clusters; }
        }
    }
}
