﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

namespace Fast.UI
{
    [Sirenix.OdinInspector.HideMonoScript]
    class FadeInFormAnimation : FormAnimation
    {
        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.Title("To fade", "All the game objects that need to fade in")]
        [SerializeField] private List<GameObject> to_fade = new List<GameObject>();
        [SerializeField] private Ease to_fade_forward_ease = Ease.InOutQuad;
        [SerializeField] private Ease to_fade_backwards_ease = Ease.InOutQuad;

        private FadeInFormAnimation() : base("FadeIn")
        {

        }

        protected override void OnAnimateForwardInternal()
        {
            Sequence sequence = DOTween.Sequence();

            for (int i = 0; i < to_fade.Count; ++i)
            {
                GameObject curr_go = to_fade[i];

                curr_go.SetActive(true);

                CanvasGroup curr_go_cg = curr_go.GetOrAddComponent<CanvasGroup>();

                Fast.Tweening.FadeTween fade_in_anim 
                    = new Fast.Tweening.FadeTween(curr_go, 0.4f, 0, 1, ForceStartingValues);

                sequence.Join(fade_in_anim.AnimateForward());
            }

            sequence.SetEase(to_fade_forward_ease);
            sequence.OnComplete(Finish);
            sequence.Play();
        }

        protected override void OnAnimateBackwardInternal()
        {
            Sequence sequence = DOTween.Sequence();

            for (int i = 0; i < to_fade.Count; ++i)
            {
                GameObject curr_go = to_fade[i];

                curr_go.SetActive(true);

                CanvasGroup curr_go_cg = curr_go.GetOrAddComponent<CanvasGroup>();

                Fast.Tweening.FadeTween fade_in_anim 
                    = new Fast.Tweening.FadeTween(curr_go, 0.4f, 0, 1, ForceStartingValues);

                sequence.Join(fade_in_anim.AnimateBackward());
            }

            sequence.SetEase(to_fade_backwards_ease);
            sequence.OnComplete(Finish);
            sequence.Play();
        }
    }
}