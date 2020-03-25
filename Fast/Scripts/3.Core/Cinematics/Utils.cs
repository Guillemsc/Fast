using FlowCanvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Cinematics
{
    public class Utils
    {
        public static Finishable GetFinishableFromFlowOutput(FlowOutput output)
        {
            BinderConnection[] connections = output.GetPortConnections();

            for (int y = 0; y < connections.Length; ++y)
            {
                BinderConnection curr_connection = connections[y];

                Finishable curr_finishable = curr_connection.targetNode as Finishable;

                if (curr_finishable != null)
                {
                    return curr_finishable;
                }
            }

            return null;
        }
    }
}
