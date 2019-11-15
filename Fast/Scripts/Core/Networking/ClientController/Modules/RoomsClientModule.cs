using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    class RoomsClientModule : ClientModule
    {
        private Callback on_join_room_success = new Callback();
        private Callback<ServerControllerError> on_join_room_fail = new Callback<ServerControllerError>();
        private Callback<PlayerLeaveRoomCause> on_disconnect_from_room = new Callback<PlayerLeaveRoomCause>();

        private bool connected_to_room = false;
        private string connected_room_id = "";

        public RoomsClientModule(ClientController client_controller) : base(client_controller)
        {

        }

        public override void OnMessageReceived(ServerControllerMessage message)
        {
            switch(message.Type)
            {
                case ServerControllerMessageType.CREATE_ROOM_RESPONSE:
                    {
                        CreateRoomResponseMessage response_message = (CreateRoomResponseMessage)message;

                        if (response_message.Success)
                        {
                            connected_to_room = true;
                            connected_room_id = response_message.RoomId;

                            on_join_room_success.Invoke();
                        }
                        else
                        {
                            connected_to_room = false;
                            connected_room_id = "";

                            on_join_room_fail.Invoke(response_message.Error);
                        }

                        break;
                    }

                case ServerControllerMessageType.JOIN_ROOM_RESPONSE:
                    {
                        JoinRoomResponseMessage response_message = (JoinRoomResponseMessage)message;

                        if (response_message.Success)
                        {
                            connected_to_room = true;
                            connected_room_id = response_message.RoomId;

                            on_join_room_success.Invoke();
                        }
                        else
                        {
                            connected_to_room = false;
                            connected_room_id = "";

                            on_join_room_fail.Invoke(response_message.Error);
                        }

                        break;
                    }

                case ServerControllerMessageType.CREATE_JOIN_ROOM_RESPONSE:
                    {
                        CreateJoinRoomResponseMessage response_message = (CreateJoinRoomResponseMessage)message;

                        if (response_message.Success)
                        {
                            connected_to_room = true;
                            connected_room_id = response_message.RoomId;

                            on_join_room_success.Invoke();
                        }
                        else
                        {
                            connected_to_room = false;
                            connected_room_id = "";

                            on_join_room_fail.Invoke(response_message.Error);
                        }

                        break;
                    }

                case ServerControllerMessageType.DISCONNECTED_FROM_ROOM:
                    {
                        DisconnectedFromRoomMessage disconnected_message = (DisconnectedFromRoomMessage)message;

                        on_disconnect_from_room.Invoke(disconnected_message.PlayerLeaveRoomCause);

                        break;
                    }
            }
        }

        public void CreateRoom(string room_name, string room_id, object join_data = null)
        {
            ClientController.SendMessage(new CreateRoomMessage(room_name, room_id, join_data));
        }

        public void JoinRoom(string room_id, object join_data = null)
        {
            ClientController.SendMessage(new JoinRoomMessage(room_id, join_data));
        }

        public void CreateJoinRoom(string room_name, string room_id, object join_data = null)
        {
            ClientController.SendMessage(new CreateJoinRoomMessage(room_name, room_id, join_data));
        }

        public void LeaveRoom()
        {
            ClientController.SendMessage(new LeaveRoomMessage());

            connected_to_room = false;
            connected_room_id = "";
        }

        public Callback OnJoinRoomSuccess
        {
            get { return on_join_room_success; }
        }

        public Callback<ServerControllerError> OnJoinRoomFail
        {
            get { return on_join_room_fail; }
        }

        public Callback<PlayerLeaveRoomCause> OnDisconnectedFromRoom
        {
            get { return on_disconnect_from_room; }
        }
    }
}
