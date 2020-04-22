using System;
using System.Collections.Generic;

namespace Fast.Logic.Commands
{
    public class LogicCommandsController : Fast.IController
    {
        public IReadOnlyList<ILogicCommand> GenerateCommands(Match.LogicMatchData match_data, IReadOnlyList<ILogicCommandInput> input)
        {
            if(input == null)
            {
                return null;
            }

            List<ILogicCommand> all_commands = new List<ILogicCommand>();

            for(int i = 0; i < input.Count; ++i)
            {
                IReadOnlyList<ILogicCommand> commands = GenerateCommand(match_data, input[i]);

                if(commands == null)
                {
                    continue;
                }

                all_commands.AddRange(commands);
            }

            return all_commands;
        }

        public IReadOnlyList<ILogicCommand> GenerateCommand(Match.LogicMatchData match_data, ILogicCommandInput input)
        {
            if(input == null)
            {
                return null;
            }

            return input.GenerateCommands(match_data);
        }

        public IReadOnlyList<ILogicCommandEffect> ExecuteCommands(Match.LogicMatchData match_data, IReadOnlyList<ILogicCommand> commands)
        {
            if(commands == null)
            { 
                return null;
            }

            List<ILogicCommandEffect> effects = new List<ILogicCommandEffect>();

            for (int i = 0; i < commands.Count; ++i)
            {
                ILogicCommand curr_command = commands[i];

                IReadOnlyList<ILogicCommandEffect> curr_command_effects = ExecuteCommand(match_data, curr_command);

                if(curr_command_effects != null)
                {
                    effects.AddRange(curr_command_effects);
                }
            }

            return effects;
        }

        public IReadOnlyList<ILogicCommandEffect> ExecuteCommand(Match.LogicMatchData match_data, ILogicCommand command)
        {
            if(command == null)
            {
                Fast.FastService.MLog.LogError(this, $"Trying to execute a command, but it's null");
                return null;
            }

            IReadOnlyList<ILogicCommandEffect> effects = command.Execute(match_data);

            return effects;
        }
    }
}
