using System;
using System.Collections.Generic;

namespace Fast
{
    public interface ICommandEffect
    {
        IReadOnlyList<ICommand> GenerateCommands();
    }
}
