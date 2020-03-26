using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace Fast.UI
{
    [Sirenix.OdinInspector.HideMonoScript]
    class ScaleAndFadeInFormAnimation : Fast.UI.FormAnimation
    {
        [Sirenix.OdinInspector.Title("To fade", "All the game objects that need to fade in")]
        [SerializeField] private List<GameObject> to_fade = new List<GameObject>();
        [SerializeField] private Ease to_fade_forward_ease = Ease.InOutQuad;
        [SerializeField] private Ease to_fade_backwards_ease = Ease.InOutQuad;

        [Sirenix.OdinInspector.Title("To scale", "All the game objects that need to scale in")]
        [SerializeField] private List<GameObject> to_scale = new List<GameObject>();
        [SerializeField] private Ease to_scale_forward_ease = Ease.InOutQuad;
        [SerializeField] private Ease to_scale_backwards_ease = Ease.InOutQuad;

        [Sirenix.OdinInspector.LabelText("To scale")]

        private ScaleAndFadeInFormAnimation() : base("ScaleAndFadeIn")
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

                Sequence seq = fade_in_anim.AnimateForward();
                seq.SetEase(to_fade_forward_ease);

                sequence.Join(seq);
            }

            for (int i = 0; i < to_scale.Count; ++i)
            {
                GameObject curr_go = to_scale[i];

                curr_go.SetActive(true);

                CanvasGroup curr_go_cg = curr_go.GetOrAddComponent<CanvasGroup>();

                Fast.Tweening.ScaleTween scale_in_anim
                    = new Fast.Tweening.ScaleTween(curr_go, 0.4f, Vector3.zero, Vector3.one, ForceStartingValues);

                Sequence seq = scale_in_anim.AnimateForward();
                seq.SetEase(to_scale_forward_ease);

                sequence.Join(seq);
            }

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

                Sequence seq = fade_in_anim.AnimateBackward();
                seq.SetEase(to_fade_backwards_ease);

                sequence.Join(seq);
            }

            for (int i = 0; i < to_scale.Count; ++i)
            {
                GameObject curr_go = to_scale[i];

                curr_go.SetActive(true);

                CanvasGroup curr_go_cg = curr_go.GetOrAddComponent<CanvasGroup>();

                Fast.Tweening.ScaleTween scale_in_anim
                    = new Fast.Tweening.ScaleTween(curr_go, 0.3f, Vector3.zero, Vector3.one, ForceStartingValues);

                Sequence seq = scale_in_anim.AnimateBackward();
                seq.SetEase(to_scale_backwards_ease);

                sequence.Join(seq);
            }

            sequence.OnComplete(Finish);
            sequence.Play();
        }
    }
}