using System;
using System.Collections.Generic;

namespace Fast.Logic
{
    public class LogicCluster 
    {
        private bool initialized = false;

        private Match.LogicMatchData match_data = null;

        public bool Initialized => initialized;

        private Callback<Commands.ILogicOutputCommand> on_output_command_sent = new Callback<Commands.ILogicOutputCommand>();

        public Callback<Commands.ILogicOutputCommand> OnOutputCommandSent => on_output_command_sent;

        public void Init(Match.LogicMatchData match_data)
        {
            if(initialized)
            {
                return;
            }

            this.match_data = match_data;

            if(this.match_data == null)
            {
                return;
            }

            initialized = true;
        }

        public void CleanUp()
        {
            if(!initialized)
            {
                return;
            }

            match_data = null;
        }

        public void ReceiveInput(Commands.ILogicInputCommand input)
        {
            if(match_data == null)
            {
                return;
            }

            if(input == null)
            {
                return;
            }

            OnInputCommandReceived(input);
        }

        protected void SendOutput(Commands.ILogicOutputCommand output)
        {
            if (match_data == null)
            {
                return;
            }

            if (output == null)
            {
                return;
            }

            on_output_command_sent.Invoke(output);
        }

        public virtual void OnInputCommandReceived(Commands.ILogicInputCommand input)
        {

        }
    }
}
