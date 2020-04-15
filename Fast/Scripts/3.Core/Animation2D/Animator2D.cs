using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.Animation2D
{
    [RequireComponent(typeof(SpriteRenderer))]
    [ExecuteAlways]
    public class Animator2D : MonoBehaviour
    {
        [Sirenix.OdinInspector.Required]
        [Sirenix.OdinInspector.LabelText("Sprite renderer")]
        [SerializeField] private SpriteRenderer sprite_renderer = null;

        [Sirenix.OdinInspector.Required]
        [Sirenix.OdinInspector.LabelText("Animation pack")]
        [SerializeField] private Animation2DPackAsset animation_pack = null;

        private Fast.Time.Timer timer = null;

        private Animation2D playing_animation = null;
        private bool playing_animation_needs_to_start = false;
        private int playing_animation_sprite_index = 0;

        private void Start()
        {
            HandleTimer();
        }

        private void Update()
        {
            HandleSpriteRenderer();

            UpdatePlayingAnimation();
        }

        private void HandleTimer()
        {
            if(!Application.isPlaying)
            {
                return;
            }

            timer = Fast.FastService.MTime.GeneralTimeContext.GetTimer();
        }

        private void HandleSpriteRenderer()
        {
            if(sprite_renderer != null)
            {
                return;
            }

            sprite_renderer = gameObject.GetOrAddComponent<SpriteRenderer>();
        }

        private Animation2D GetAnimation(string animation_name)
        {
            if(animation_pack == null)
            {
                return null;
            }

            for(int i = 0; i < animation_pack.Animations.Count; ++i)
            {
                Animation2D curr_animation = animation_pack.Animations[i];

                if(curr_animation.Name == animation_name)
                {
                    return curr_animation;
                }
            }

            return null;
        }

        public void SetTimer(Fast.Time.Timer timer)
        {
            if(timer == null)
            {
                return;
            }

            this.timer = timer;
        }

        public void SetXFlip(bool flip)
        {
            if(sprite_renderer == null)
            {
                return;
            }

            sprite_renderer.flipX = flip;
        }

        public void PlayAnimation(string name)
        {
            if (sprite_renderer == null)
            {
                return;
            }

            if (playing_animation != null)
            {
                if(playing_animation.Name == name)
                {
                    return;
                }
            }

            Animation2D animation_to_play = GetAnimation(name);

            if(animation_to_play == null)
            {
                return;
            }

            playing_animation = animation_to_play;
            playing_animation_needs_to_start = true;
        }

        private void UpdatePlayingAnimation()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (playing_animation == null)
            {
                return;
            }

            if(playing_animation_needs_to_start)
            {
                playing_animation_needs_to_start = false;

                playing_animation_sprite_index = 0;

                timer.Start();
            }

            if(timer.ReadTime().TotalSeconds > playing_animation.PlaybackSpeed)
            {
                ++playing_animation_sprite_index;

                if (playing_animation_sprite_index >= playing_animation.Sprites.Count)
                {
                    if (playing_animation.Loop)
                    {
                        playing_animation_sprite_index = 0;
                    }
                    else
                    {
                        timer.Reset();
                    }
                }

                if (sprite_renderer != null)
                {
                    sprite_renderer.sprite = playing_animation.Sprites[playing_animation_sprite_index];
                }

                timer.Start();
            }
        }
    }
}
