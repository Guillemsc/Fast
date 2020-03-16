using System;
using System.Collections.Generic;

namespace Fast.Commands
{
    public interface ICommand
    {
        bool CanExecute();
        IReadOnlyList<ICommandEffect> Execute();
        IReadOnlyList<ICommand> GenerateCommands();
    }
}
