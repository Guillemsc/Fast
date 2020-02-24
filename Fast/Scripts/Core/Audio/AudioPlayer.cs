using System.Collections.Generic;
using UnityEngine;

namespace Fast.Audio
{
    class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private int max_audio_pool = 20;
        [SerializeField] private float update_time = 4.0f;

        private Timer update_timer = new Timer();

        private List<AudioPlaying> audios_playing = new List<AudioPlaying>();
        private List<AudioSource> audios_pool = new List<AudioSource>();

        private bool muted = false;

        private class AudioPlaying
        {
            public AudioClip clip = null;
            public AudioSource source = null;
        }

        private void Start()
        {
            update_timer.Start();
        }

        private void Update()
        {
            if (update_timer.ReadTime() >= update_time)
            {
                update_timer.Reset();

                UpdateAudiosPlaying();

                update_timer.Start();
            }
        }

        public bool Muted
        {
            get { return muted; }
            set { muted = value; }
        }

        public void PlayAudio(AudioClip clip, float volume)
        {
            if (clip != null)
            {
                if (!muted)
                {
                    AudioSource audio_source = GetAudioSource();
                    audio_source.volume = volume;
                    audio_source.clip = clip;

                    AudioPlaying audio_playing = new AudioPlaying();
                    audio_playing.clip = clip;
                    audio_playing.source = audio_source;

                    audio_source.Play();

                    audios_playing.Add(audio_playing);
                }
            }
        }

        private void UpdateAudiosPlaying()
        {
            for (int i = 0; i < audios_playing.Count;)
            {
                AudioPlaying curr_audio = audios_playing[i];

                if (!curr_audio.source.isPlaying)
                {
                    RetrieveAudioSource(curr_audio.source);

                    audios_playing.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }
        }

        private AudioSource GetAudioSource()
        {
            AudioSource ret = null;

            if (audios_pool.Count > 0)
            {
                ret = audios_pool[0];

                audios_pool.RemoveAt(0);
            }
            else
            {
                ret = gameObject.AddComponent<AudioSource>();
            }

            return ret;
        }

        private void RetrieveAudioSource(AudioSource audio_source)
        {
            if (audios_pool.Count < max_audio_pool)
            {
                audios_pool.Add(audio_source);
            }
            else
            {
                Destroy(audio_source);
            }
        }
    }
}
