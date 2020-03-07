using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Modules
{
    public class ParticlesModule : UpdatableModule
    {
        class ParticlePlaying
        {
            public ParticlePlaying(Fast.Particles.ParticlePlayer player)
            {
                this.player = player;
            }

            private Fast.Particles.ParticlePlayer player = null;
            private bool needs_to_play = false;
            private Fast.Callback on_finish = new Callback();

            public Fast.Particles.ParticlePlayer Player
            {
                get { return player; }
            }

            public bool NeedsToPlay
            {
                get { return needs_to_play; }
                set { needs_to_play = value; }
            }

            public Fast.Callback OnFinish
            {
                get { return on_finish; }
            }
        }

        private List<ParticlePlaying> particles_running = new List<ParticlePlaying>();

        public ParticlesModule(FastService fast) : base(fast)
        {

        }

        public override void Update()
        {
            UpdatePlayingParticles();
        }

        public void PlayParticle(Fast.Particles.ParticlePlayer player, Action on_finish = null)
        {
            if(player != null)
            {
                ParticlePlaying playing = new ParticlePlaying(player);
                playing.NeedsToPlay = true;

                playing.OnFinish.Subscribe(on_finish);

                particles_running.Add(playing);
            }
        }

        private void UpdatePlayingParticles()
        {
            List<ParticlePlaying> to_update = new List<ParticlePlaying>(particles_running);

            List<ParticlePlaying> to_remove = new List<ParticlePlaying>();

            for (int i = 0; i < to_update.Count; ++i)
            {
                ParticlePlaying curr_playing = to_update[i];

                if (curr_playing.NeedsToPlay)
                {
                    curr_playing.NeedsToPlay = false;

                    curr_playing.Player.Stop();
                    curr_playing.Player.Play();
                }
                else
                {
                    if (!curr_playing.Player.IsPlaying())
                    {
                        to_remove.Add(curr_playing);
                    }
                }
            }

            for (int i = 0; i < to_remove.Count; ++i)
            {
                ParticlePlaying curr_to_remove = to_remove[i];

                for (int y = 0; y < particles_running.Count; ++y)
                {
                    if(particles_running[y] == curr_to_remove)
                    {
                        curr_to_remove.OnFinish.Invoke();

                        particles_running.RemoveAt(y);

                        break;
                    }
                }
            }
        }
    }
}
