using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    [System.Serializable]
    class MatchmakingFinishedMessage : ServerControllerMessage
    {
        private bool success = false;

        private string cluster_id = "";

        public MatchmakingFinishedMessage(bool success, string cluster_id = "") : base(ServerControllerMessageType.MATCHMAKING_FINISHED)
        {
            this.success = success;
            this.cluster_id = cluster_id;
        }

        public bool MatchmakingSuccess
        {
            get { return success; }
        }

        public string ClusterId
        {
            get { return cluster_id; }
        }
    }
}
