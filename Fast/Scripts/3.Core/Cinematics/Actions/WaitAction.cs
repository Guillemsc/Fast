using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Cinematics
{
    public class WaitAction : CinematicAction
    {
        private float time = 0.0f;

        public WaitAction(float time)
        {
            this.time = time;
        }
    }
}
