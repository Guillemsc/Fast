using System;
using System.Collections.Generic;

namespace Fast.Commands
{
    public class CommandsController : Fast.IController
    {
        public IReadOnlyList<ICommandEffect> ExecuteCommands(IReadOnlyList<ICommand> commands)
        {
            if(commands == null)
            {
                Fast.FastService.MLog.LogError(this, $"Trying to get a execute commands, but list is null");
                return null;
            }

            List<ICommandEffect> effects = new List<ICommandEffect>();

            for (int i = 0; i < commands.Count; ++i)
            {
                ICommand curr_command = commands[i];

                IReadOnlyList<ICommandEffect> curr_command_effects = ExecuteCommand(curr_command);

                effects.AddRange(curr_command_effects);
            }

            return effects;
        }

        public IReadOnlyList<ICommandEffect> ExecuteCommand(ICommand command)
        {
            if(command == null)
            {
                Fast.FastService.MLog.LogError(this, $"Trying to execute a command, but it's null");
                return null;
            }

            List<ICommandEffect> effects = new List<ICommandEffect>();

            Queue<ICommand> commands_queue = new Queue<ICommand>();

            commands_queue.Enqueue(command);

            while (commands_queue.Count > 0)
            {
                ICommand curr_command = commands_queue.Dequeue();

                IReadOnlyList<ICommandEffect> curr_command_effects = curr_command.Execute();
                effects.AddRange(curr_command_effects);

                for (int i = 0; i < curr_command_effects.Count; ++i)
                {
                    ICommandEffect curr_effect = curr_command_effects[i];

                    IReadOnlyList<ICommand> curr_effect_new_commands = curr_effect.GenerateCommands();

                    AddCommandsToCommandsQueue(ref commands_queue, curr_effect_new_commands);
                }

                IReadOnlyList<ICommand> curr_command_new_commands = curr_command.GenerateCommands();

                AddCommandsToCommandsQueue(ref commands_queue, curr_command_new_commands);
            }

            return effects;
        }

        public IReadOnlyList<ICommandEffect> ExecuteCommandInput(ICommandInput input)
        {
            if(input == null)
            {
                Fast.FastService.MLog.LogError(this, $"Trying to get a execute command  input, but it's null");
                return null;
            }

            IReadOnlyList<ICommand> commands = input.GenerateCommands();

            return ExecuteCommands(commands);
        }

        private void AddCommandsToCommandsQueue(ref Queue<ICommand> commands_queue, IReadOnlyList<ICommand> commands)
        {
            for(int i = 0; i < commands.Count; ++i)
            {
                commands_queue.Enqueue(commands[i]);
            }
        }
    }
}
