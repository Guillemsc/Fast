using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast
{
    public class CommandsController
    {
        public IReadOnlyList<ICommandEffect> ExecuteCommands(IReadOnlyList<ICommand> commands)
        {
            List<ICommandEffect> ret = new List<ICommandEffect>();

            for (int i = 0; i < commands.Count; ++i)
            {
                ICommand curr_command = commands[i];

                IReadOnlyList<ICommandEffect> curr_command_effects = ExecuteCommand(curr_command);

                ret.AddRange(curr_command_effects);
            }

            return ret.AsReadOnly();
        }

        public IReadOnlyList<ICommandEffect> ExecuteCommand(ICommand command)
        {
            List<ICommandEffect> ret = new List<ICommandEffect>();

            Queue<ICommand> commands_queue = new Queue<ICommand>();

            commands_queue.Enqueue(command);

            while (commands_queue.Count > 0)
            {
                ICommand curr_command = commands_queue.Dequeue();

                IReadOnlyList<ICommandEffect> curr_command_effects = curr_command.Execute();
                ret.AddRange(curr_command_effects);

                for (int i = 0; i < curr_command_effects.Count; ++i)
                {
                    ICommandEffect curr_effect = curr_command_effects[i];

                    IReadOnlyList<ICommand> curr_effect_new_commands = curr_effect.GenerateCommands();

                    AddCommandsToCommandsQueue(ref commands_queue, curr_effect_new_commands);
                }

                IReadOnlyList<ICommand> curr_command_new_commands = curr_command.GenerateCommands();

                AddCommandsToCommandsQueue(ref commands_queue, curr_command_new_commands);
            }

            return ret.AsReadOnly();
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
