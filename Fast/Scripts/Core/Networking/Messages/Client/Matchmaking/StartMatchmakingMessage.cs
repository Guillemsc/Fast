using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    [System.Serializable]
    class StartMatchmakingMessage : ServerControllerMessage
    {
        private MatchmakingData data = null;

        public StartMatchmakingMessage(MatchmakingData data) : base(ServerControllerMessageType.START_MATCHMAKING)
        {
            this.data = data;
        }

        public MatchmakingData MatchmakingData
        {
            get { return data; }
        }
    }
}
