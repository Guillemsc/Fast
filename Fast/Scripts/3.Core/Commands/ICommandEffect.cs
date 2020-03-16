using System;
using System.Collections.Generic;

namespace Fast.Commands
{
    public interface ICommandEffect
    {
        IReadOnlyList<ICommand> GenerateCommands();
    }
}
