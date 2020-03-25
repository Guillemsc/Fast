using System;
using System.Collections.Generic;

namespace Fast.Commands
{
    public interface ICommandInput
    {
        IReadOnlyList<ICommand> GenerateCommands();
    }
}
