using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.FlowCommands
{
    public class FlowCommandsController : Fast.IController, IUpdatable
    {
        private readonly List<FlowCommand> to_execute = new List<FlowCommand>();

        public void Update()
        {
            UpdateCommands();
        }

        public void ExecuteCommand(FlowCommand command)
        {
            if(command == null)
            {
                return;
            }

            IReadOnlyList<FlowCommand> to_pre_add = command.PreAddCommands();
            to_execute.AddRange(to_pre_add);

            to_execute.Add(command);

            IReadOnlyList<FlowCommand> to_post_add = command.PostAddCommands();
            to_execute.AddRange(to_post_add);
        }

        private void UpdateCommands()
        {
            if(to_execute.Count <= 0)
            {
                return;
            }

            FlowCommand curr_executing = to_execute[0];

            if (curr_executing.Finished)
            {
                to_execute.RemoveAt(0);
            }
            else if (!curr_executing.Started)
            {
                curr_executing.Execute();

                Fast.FastService.MLog.LogInfo($"Starting flow command [{curr_executing.GetType().ToString()}]: { curr_executing.LogCommand()}");
            }
        }
    }
}
