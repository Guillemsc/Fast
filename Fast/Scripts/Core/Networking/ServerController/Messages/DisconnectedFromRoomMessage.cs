using System;

namespace Fast.Networking
{
    [System.Serializable]
    public class DisconnectedFromRoomMessage : ServerControllerMessage
    {
        private PlayerLeaveRoomCause cause = PlayerLeaveRoomCause.UNKNOWN;

        public DisconnectedFromRoomMessage(PlayerLeaveRoomCause cause) : base(ServerControllerMessageType.DISCONNECTED_FROM_ROOM)
        {
            this.cause = cause;
        }

        public PlayerLeaveRoomCause PlayerLeaveRoomCause
        {
            get { return cause; }
        }
    }
}
