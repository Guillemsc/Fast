using System;
using System.Collections.Generic;

namespace Fast.Logic
{
    public class LogicCluster 
    {
        private bool initialized = false;

        private Match.LogicMatchData match_data = null;

        private readonly Commands.LogicCommandsController controller = new Commands.LogicCommandsController();

        private readonly List<Commands.ILogicCommandInput> received_input = new List<Commands.ILogicCommandInput>();
        private readonly List<Commands.ILogicCommandEffect> generated_effects = new List<Commands.ILogicCommandEffect>();

        public bool Initialized => initialized;

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
            received_input.Clear();
            generated_effects.Clear();
        }

        public void PushInput(Commands.ILogicCommandInput input)
        {
            if(match_data == null)
            {
                return;
            }

            if(input == null)
            {
                return;
            }

            received_input.Add(input);
        }

        public void UpdateLogic()
        {
            if (match_data == null)
            {
                return;
            }

            IReadOnlyList<Commands.ILogicCommandInput> input = new List<Commands.ILogicCommandInput>(received_input);

            received_input.Clear();

            IReadOnlyList<Commands.ILogicCommand> commands = controller.GenerateCommands(match_data, input);

            IReadOnlyList<Commands.ILogicCommandEffect> effects = controller.ExecuteCommands(match_data, commands);

            generated_effects.AddRange(effects);
        }

        public IReadOnlyList<Commands.ILogicCommandEffect> PopEffects()
        {
            List<Commands.ILogicCommandEffect> effects = new List<Commands.ILogicCommandEffect>(generated_effects);

            generated_effects.Clear();

            return effects;
        }
    }
}
