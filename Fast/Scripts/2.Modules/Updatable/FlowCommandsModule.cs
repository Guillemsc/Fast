using System;

namespace Fast.Modules
{
    public class FlowCommandsModule : UpdatableModule
    {
        private readonly Fast.FlowCommands.FlowCommandsController controller = new FlowCommands.FlowCommandsController();

        public override void Update()
        {
            controller.Update();
        }

        public void ExecuteCommand(Fast.FlowCommands.FlowCommand command)
        {
            controller.ExecuteCommand(command);
        }
    }
}
