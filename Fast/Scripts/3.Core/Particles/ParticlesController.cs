using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Particles 
{
    class ParticlesController : Fast.IController, Fast.IUpdatable
    {
        private readonly List<ParticlePlaying> particles_running = new List<ParticlePlaying>();

        public void Update()
        {
            UpdatePlayingParticles();
        }

        public void PlayParticle(ParticlePlayer player, Action on_finish = null)
        {
            if (player != null)
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
                    if (particles_running[y] == curr_to_remove)
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
