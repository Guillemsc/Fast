using System;
using System.Collections.Generic;

namespace Fast.Logic.Commands
{
    public interface ILogicCommand
    {
        IReadOnlyList<ILogicCommandEffect> Execute(Match.LogicMatchData match_data);
    }
}
