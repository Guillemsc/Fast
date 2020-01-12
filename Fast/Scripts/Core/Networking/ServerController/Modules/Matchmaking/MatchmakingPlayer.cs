using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public class MatchmakingPlayer
    {
        private Player player = null;

        private int elo = 0;

        public MatchmakingPlayer(Player player)
        {
            this.player = player;
        }

        public Player Player
        {
            get { return player; }
        }

        public int Elo
        {
            get { return elo; }
            set { elo = value; }
        }
    }
}
