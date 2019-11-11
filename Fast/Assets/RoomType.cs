using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fast.Networking;

[Fast.Networking.RoomName("RoomTest")]
class RoomType : Fast.Networking.Room
{
    protected override void OnPlayerConnected(RoomPlayer player)
    {
        player.SendMessage("despacito");
    }
}

