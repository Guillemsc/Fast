using System;
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
        [SerializeField] private List<GameObject> to_fade = new List<GameObject>();

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

                Fast.Animations.FadeAnimation fade_in_anim 
                    = new Fast.Animations.FadeAnimation(curr_go, 3.4f, 0, 1, ForceStartingValues);

                sequence.Join(fade_in_anim.AnimateForward());
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

                Fast.Animations.FadeAnimation fade_in_anim 
                    = new Fast.Animations.FadeAnimation(curr_go, 3.4f, 0, 1, ForceStartingValues);

                sequence.Join(fade_in_anim.AnimateBackward());
            }

            sequence.OnComplete(Finish);
            sequence.Play();
        }
    }
}