using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Networking
{
    public class MatchmakingClientModule : ClientModule
    {
        private Callback<string> on_matchmaking_success = new Callback<string>();
        private Callback on_matchmaking_error = new Callback();

        public MatchmakingClientModule(ClientController client_controller) : base(client_controller)
        {

        }

        public override void OnMessageReceived(ServerControllerMessage message)
        {
            switch (message.Type)
            {
                case ServerControllerMessageType.MATCHMAKING_FINISHED:
                    {
                        MatchmakingFinishedMessage matchmaking_message = (MatchmakingFinishedMessage)message;

                        if(matchmaking_message.MatchmakingSuccess)
                        {
                            if (on_matchmaking_success != null)
                                on_matchmaking_success.Invoke(matchmaking_message.ClusterId);
                        }
                        else
                        {
                            if (on_matchmaking_error != null)
                                on_matchmaking_error.Invoke();
                        }

                        break;
                    }
            }
        }

        public void StartMatchmaking(MatchmakingData data, Action<string> on_success, Action on_fail)
        {
            on_matchmaking_success.UnSubscribeAll();
            on_matchmaking_success.Subscribe(on_success);

            on_matchmaking_error.UnSubscribeAll();
            on_matchmaking_error.Subscribe(on_fail);

            data.AddPartyClientId(ClientController.ClientId);

            ClientController.SendMessage(new StartMatchmakingMessage(data));
        }
    }
}
