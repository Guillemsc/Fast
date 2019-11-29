using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public class BaseRoom
    {
        private RoomsServerModule rooms_module = null;

        private RoomSettings settings = null;

        private string room_id = "";

        protected bool finished_updating = true;

        public void Init(RoomsServerModule rooms_module, RoomSettings settings, string room_id)
        {
            this.rooms_module = rooms_module;
            this.settings = settings;
            this.room_id = room_id;
        }

        public RoomsServerModule RoomsServerModule
        {
            get { return rooms_module; }
        }

        public string RoomName
        {
            get { return settings.Name; }
        }

        public string RoomId
        {
            get { return room_id; }
        }

        public virtual bool IsEmpty()
        {
            return true;
        }

        public bool _Updatable
        {
            get { return settings.Updatable; }
        }

        public bool _FinishedUpdating
        {
            get { return finished_updating; }
        }

        public virtual void PlayerConnect(Player player, object join_data, Action on_success, Action<ServerControllerError> on_error)
        {
            if (on_error != null)
                on_error.Invoke(ServerControllerError.EMPTY);
        }

        public virtual void PlayerDisconnect(Player player)
        {

        }

        public virtual void MessageReceived(Player player, object message_obj)
        {

        }

        public virtual void Update()
        {

        }
    }
}
