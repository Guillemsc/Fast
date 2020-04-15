using System;
using System.Collections.Generic;

namespace Fast.Commands
{
    public interface ICommand
    {
        IReadOnlyList<ICommand> GeneratePreCommands();
        bool CanExecute();
        IReadOnlyList<ICommandEffect> Execute();
        IReadOnlyList<ICommand> GeneratePostCommands();
    }
}
