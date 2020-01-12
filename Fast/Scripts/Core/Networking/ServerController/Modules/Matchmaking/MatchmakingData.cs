using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    [System.Serializable]
    public class MatchmakingData
    {
        private string game_mode_id = "";
        private List<int> clients_id = new List<int>();

        public MatchmakingData(string game_mode_id)
        {
            this.game_mode_id = game_mode_id;
        }

        public void AddPartyClientId(int client_id)
        {
            clients_id.Add(client_id);
        }

        public string GameModeId
        {
            get { return game_mode_id; }
        }

        public List<int> ClientsId
        {
            get { return clients_id; }
        }
    }
}
