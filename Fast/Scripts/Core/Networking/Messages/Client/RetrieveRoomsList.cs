using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    [System.Serializable]
    public class RetrieveRoomsList : ServerControllerMessage
    {
        private string room_name = "";

        public RetrieveRoomsList(string room_name) : base(ServerControllerMessageType.RETRIEVE_ROOMS_LIST)
        {
            this.room_name = room_name;
        }

        public string RoomName
        {
            get { return room_name; }
        }
    }
}
