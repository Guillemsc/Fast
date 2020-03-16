using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Particles
{
    class ParticlePlaying
    {
        private readonly ParticlePlayer player = null;
        private bool needs_to_play = false;
        private readonly Fast.Callback on_finish = new Callback();

        public ParticlePlaying(ParticlePlayer player)
        {
            this.player = player;
        }

        public ParticlePlayer Player => player;

        public bool NeedsToPlay
        {
            get { return needs_to_play; }
            set { needs_to_play = value; }
        }

        public Fast.Callback OnFinish => on_finish;
    }
}
