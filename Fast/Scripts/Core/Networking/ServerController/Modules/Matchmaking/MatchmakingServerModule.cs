using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    class MatchmakingServerModule : ServerModule
    {
        private List<MatchmakingGameModeSettings> game_modes_settings = new List<MatchmakingGameModeSettings>();

        private List<MatchmakingGameMode> matchmaking_game_modes = new List<MatchmakingGameMode>();

        private Fast.RandomGenerator random_generator = new RandomGenerator();

        private bool finished_matchmaking = true;
        private Fast.Networking.Timer start_matchmaking_timer = new Timer();
        private float start_matchmaking_time = 4.0f;

        public MatchmakingServerModule(ServerController server_controller) : base(server_controller)
        {
           
        }

        public override void Start()
        {
            GetGameModeSettings();

            finished_matchmaking = true;
            start_matchmaking_timer.Start();
        }

        public override void Update()
        {
            CheckPerformMatchmaking();
        }

        public override void OnPlayerDisconnected(Player player)
        {
            RemoveCluster(player);
        }

        private void GetGameModeSettings()
        {
            game_modes_settings.Clear();

            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < assemblies.Length; ++i)
            {
                System.Reflection.Assembly curr_assembly = assemblies[i];

                Type[] types = curr_assembly.GetTypes();

                for (int y = 0; y < types.Length; ++y)
                {
                    Type curr_type = types[y];

                    object[] attributes_settings = curr_type.GetCustomAttributes(typeof(MatchmakingGameModeSettings), true);

                    if (attributes_settings != null && attributes_settings.Length > 0)
                    {
                        List<MatchmakingGameModeSettings> attributes = attributes_settings.Cast<MatchmakingGameModeSettings>().ToList();

                        if (attributes.Count > 0)
                        {
                            game_modes_settings.Add(attributes[0]);
                        }
                    }
                }
            }
        }

        public override void OnMessageReceived(Player player, ServerControllerMessage server_message)
        {
            switch(server_message.Type)
            {
                case ServerControllerMessageType.START_MATCHMAKING:
                    {
                        StartMatchmakingMessage mes = (StartMatchmakingMessage)server_message;
                        
                        bool can_add = AddCluster(player, mes.MatchmakingData);

                        if (!can_add)
                        {
                            ServerController.SendMessage(player, new MatchmakingFinishedMessage(false));
                        }

                        break;
                    }
            }
        }

        private MatchmakingGameModeSettings GetMatchmakingGameModeSettings(string matchmaking_game_mode_id)
        {
            MatchmakingGameModeSettings ret = null;

            for(int i = 0; i < game_modes_settings.Count; ++i)
            {
                MatchmakingGameModeSettings curr_settings = game_modes_settings[i];

                if(curr_settings.GameModeId == matchmaking_game_mode_id)
                {
                    ret = curr_settings;

                    break;
                }
            }

            return ret;
        }

        private bool AddCluster(Player player, MatchmakingData data)
        {
            bool ret = true;

            MatchmakingGameModeSettings settings = GetMatchmakingGameModeSettings(data.GameModeId);

            if(settings != null)
            {
                MatchmakingPlayer party_owner = new MatchmakingPlayer(player);

                MatchmakingParty matchmaking_party = new MatchmakingParty(party_owner, settings);

                for(int i = 0; i < data.ClientsId.Count; ++i)
                {
                    int client_id = data.ClientsId[i];

                    Player curr_player = ServerController.GetPlayer(client_id);

                    if (curr_player != null)
                    {
                        if (!curr_player.OnMatchmaking)
                        {
                            if (curr_player.ClientId != player.ClientId)
                            {
                                MatchmakingPlayer matchmaking_player = new MatchmakingPlayer(player);

                                matchmaking_party.AddPlayer(matchmaking_player);
                            }
                            else
                            {
                                matchmaking_party.AddPlayer(party_owner);
                            }
                        }
                        else
                        {
                            ret = false;
                        }
                    }
                    else
                    {
                        ret = false;
                    }
                }

                if (ret)
                {
                    MatchmakingCluster matchmaking_cluster = new MatchmakingCluster(settings);

                    matchmaking_cluster.AddParty(matchmaking_party);

                    MatchmakingGameMode mode_to_add = null;

                    lock (matchmaking_game_modes)
                    {
                        for (int i = 0; i < matchmaking_game_modes.Count; ++i)
                        {
                            MatchmakingGameMode curr_game_mode = matchmaking_game_modes[i];

                            bool equal = GameModeSettingsEqual(curr_game_mode.MatchmakingGameModeSettings, settings);

                            if (equal)
                            {
                                mode_to_add = curr_game_mode;

                                break;
                            }
                        }

                        if (mode_to_add == null)
                        {
                            mode_to_add = new MatchmakingGameMode(settings);

                            matchmaking_game_modes.Add(mode_to_add);
                        }

                        for(int i = 0; i < matchmaking_party.MatchmakingPlayers.Count; ++i)
                        {
                            MatchmakingPlayer curr_matchmaking_player = matchmaking_party.MatchmakingPlayers[i];

                            curr_matchmaking_player.Player.OnMatchmaking = true;
                        }

                        mode_to_add.AddCluster(matchmaking_cluster);

                        Logger.ServerLogInfo("Player with id: " + player.ClientId + "started matchmaking " +
                            "| Party count: " + matchmaking_party.MatchmakingPlayers.Count);
                    }
                }
            }

            return ret;
        }

        private void RemoveCluster(Player player)
        {
            lock (matchmaking_game_modes)
            {
                if (player.OnMatchmaking)
                {
                    bool found = false;

                    MatchmakingParty found_party = null;

                    for (int i = 0; i < matchmaking_game_modes.Count; ++i)
                    {
                        MatchmakingGameMode curr_mode = matchmaking_game_modes[i];

                        for(int y = 0; y < curr_mode.Clusters.Count; ++y)
                        {
                            MatchmakingCluster curr_cluster = curr_mode.Clusters[y];

                            for(int z = 0; z < curr_cluster.Parties.Count; ++z)
                            {
                                MatchmakingParty curr_party = curr_cluster.Parties[z];

                                for(int v = 0; v < curr_party.MatchmakingPlayers.Count; ++v)
                                {
                                    MatchmakingPlayer curr_player = curr_party.MatchmakingPlayers[v];

                                    if(curr_player.Player == player)
                                    {
                                        found_party = curr_party;

                                        curr_cluster.Parties.RemoveAt(z);

                                        if(curr_cluster.Parties.Count == 0)
                                        {
                                            curr_mode.Clusters.RemoveAt(y);
                                        }

                                        found = true;

                                        break;
                                    }
                                }

                                if(found)
                                {
                                    break;
                                }
                            }

                            if (found)
                            {
                                break;
                            }
                        }

                        if (found)
                        {
                            break;
                        }
                    }

                    if(found_party != null)
                    {
                        for (int i = 0; i < found_party.MatchmakingPlayers.Count; ++i)
                        {
                            MatchmakingPlayer curr_player = found_party.MatchmakingPlayers[i];

                            curr_player.Player.OnMatchmaking = false;
                        }

                        Logger.ServerLogInfo("Player stoped matchmaking: " + player.ClientId + " | Party count: " + found_party.MatchmakingPlayers.Count);
                    }
                }
            }
        }

        private void CheckPerformMatchmaking()
        {
            lock (matchmaking_game_modes)
            {
                if (finished_matchmaking)
                {
                    if (start_matchmaking_timer.ReadTime() > start_matchmaking_time)
                    {
                        start_matchmaking_timer.Reset();
                        start_matchmaking_timer.Start();

                        finished_matchmaking = false;

                        Task.Factory.StartNew(() => PerformMatchmaking()).
                        ContinueWith(delegate (Task update_task)
                        {
                            string error_msg = "";
                            Exception exception = null;

                            bool has_errors = update_task.HasErrors(out error_msg, out exception);

                            if (has_errors)
                            {
                                Logger.ServerLogError(ToString() + " PerformMatchmaking(): " + error_msg);
                            }

                            finished_matchmaking = true;
                        });
                    }
                }
            }
        }

        private void PerformMatchmaking()
        {
            List<MatchmakingCluster> complete_clusters = new List<MatchmakingCluster>();

            lock (matchmaking_game_modes)
            {
                for (int i = 0; i < matchmaking_game_modes.Count; ++i)
                {
                    MatchmakingGameMode curr_mode = matchmaking_game_modes[i];

                    List<MatchmakingCluster> clusters_to_check = new List<MatchmakingCluster>(curr_mode.Clusters);

                    while (clusters_to_check.Count > 0)
                    {
                        MatchmakingCluster curr_cluster = clusters_to_check[0];

                        if (!curr_cluster.Complete)
                        {
                            clusters_to_check.RemoveAt(0);

                            for (int y = 0; y < clusters_to_check.Count;)
                            {
                                MatchmakingCluster curr_checking_cluster = clusters_to_check[y];

                                if (!curr_checking_cluster.Complete)
                                {
                                    bool equal = GameModeSettingsEqual(curr_cluster.MatchmakingGameModeSettings,
                                        curr_checking_cluster.MatchmakingGameModeSettings);

                                    if (equal)
                                    {
                                        bool can_merge = ClustersCanMerge(curr_cluster, curr_checking_cluster);

                                        if (can_merge)
                                        {
                                            curr_mode.MergeClusters(curr_cluster, curr_checking_cluster);

                                            clusters_to_check.RemoveAt(y);

                                            bool merge_completes = IsClusterComplete(curr_cluster);

                                            if (merge_completes)
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            ++y;
                                        }
                                    }
                                    else
                                    {
                                        ++y;
                                    }
                                }
                            }
                        }

                        bool complete = IsClusterComplete(curr_cluster);

                        if (complete)
                        {
                            curr_cluster.Complete = true;

                            curr_mode.RemoveCluster(curr_cluster);

                            complete_clusters.Add(curr_cluster);
                        }
                    }
                }

                for (int i = 0; i < complete_clusters.Count; ++i)
                {
                    MatchmakingCluster curr_cluster = complete_clusters[i];

                    FinishClusterMatchmaking(curr_cluster);
                }
            }
        }

        public bool GameModeSettingsEqual(MatchmakingGameModeSettings s1, MatchmakingGameModeSettings s2)
        {
            bool ret = false;

            if(s1.GameModeId == s2.GameModeId)
            {
                if(s1.MinimumPlayers == s2.MinimumPlayers)
                {
                    if (s1.MaximumPlayers == s2.MaximumPlayers)
                    {
                        ret = true;
                    }
                }
            }

            return ret;
        }

        public bool ClustersCanMerge(MatchmakingCluster c1, MatchmakingCluster c2)
        {
            bool ret = false;

            if(c1.TotalPlayers + c2.TotalPlayers <= c1.MatchmakingGameModeSettings.MaximumPlayers)
            {
                ret = true;
            }

            return ret;
        }

        public bool IsClusterComplete(MatchmakingCluster c1)
        {
            bool ret = false;

            if(c1.MatchmakingGameModeSettings.MaximumPlayers == c1.TotalPlayers)
            {
                ret = true;
            }

            return ret;
        }

        private void FinishClusterMatchmaking(MatchmakingCluster cluster)
        {
            string random_id = random_generator.GetInt().ToString();

            for (int i = 0; i < cluster.Parties.Count; ++i)
            {
                MatchmakingParty curr_party = cluster.Parties[i];

                for(int y = 0; y < curr_party.MatchmakingPlayers.Count; ++y)
                {
                    MatchmakingPlayer curr_player = curr_party.MatchmakingPlayers[y];

                    curr_player.Player.OnMatchmaking = false;
                }

                ServerController.SendMessage(curr_party.PartyOwner.Player, new MatchmakingFinishedMessage(true, random_id));
            }

            Logger.ServerLogInfo("Matchmaking success | Players count: " + cluster.TotalPlayers + " | Return id: " + random_id);
        }
    }
}
