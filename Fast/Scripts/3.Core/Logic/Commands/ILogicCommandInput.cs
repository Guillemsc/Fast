using System;
using System.Collections.Generic;

namespace Fast.Logic.Commands
{
    public interface ILogicCommandInput
    {
        IReadOnlyList<ILogicCommand> GenerateCommands(Match.LogicMatchData match_data);
    }
}
