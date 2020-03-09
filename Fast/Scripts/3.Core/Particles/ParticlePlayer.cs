using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Particles
{
    public class ParticlePlayer : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> particles = new List<ParticleSystem>();

        private void Start()
        {
            Stop();

            for (int i = 0; i < particles.Count; ++i)
            {
                ParticleSystem curr_system = particles[i];

                ParticleSystem.MainModule main = curr_system.main;
                ParticleSystem.EmissionModule emission = curr_system.emission;

                main.loop = false;
                main.playOnAwake = false;

                emission.enabled = true;
            }
        }

        public void Play()
        {
            for (int i = 0; i < particles.Count; ++i)
            {
                ParticleSystem curr_system = particles[i];

                curr_system.Play();
            }
        }

        public void Stop()
        {
            for (int i = 0; i < particles.Count; ++i)
            {
                ParticleSystem curr_system = particles[i];

                curr_system.Stop();
            }
        }

        public bool IsPlaying()
        {
            bool ret = false;

            for (int i = 0; i < particles.Count; ++i)
            {
                ret = particles[i].isPlaying;

                if(ret)
                {
                    break;
                }
            }

            return ret;
        }
    }
}
