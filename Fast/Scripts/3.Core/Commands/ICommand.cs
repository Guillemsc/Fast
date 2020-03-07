using System;
using System.Collections.Generic;

namespace Fast
{
    public interface ICommand
    {
        bool CanExecute();
        IReadOnlyList<ICommandEffect> Execute();
        IReadOnlyList<ICommand> GenerateCommands();
    }
}
