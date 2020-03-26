using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Fast.Tweening
{
    class FadeTween : Tween
    {
        private GameObject go = null;
        private float time = 0.0f;

        private float start_alpha = 0.0f;
        private float end_alpha = 0.0f;

        public FadeTween(GameObject go, float time, float start_alpha, float end_alpha, 
            bool force_start_value = false) : base(force_start_value)
        {
            this.go = go;
            this.time = time;
            this.start_alpha = start_alpha;
            this.end_alpha = end_alpha;
        }

        public override Sequence AnimateForward()
        {
            Sequence ret = DOTween.Sequence();

            CanvasGroup cg = go.GetOrAddComponent<CanvasGroup>();

            if (ForceStartValues)
            {
                ret.Append(cg.DOFade(start_alpha, 0.0f));
            }

            ret.Append(cg.DOFade(end_alpha, time).SetEase(Ease.InOutQuad));

            return ret;
        }

        public override Sequence AnimateBackward()
        {
            Sequence ret = DOTween.Sequence();

            CanvasGroup cg = go.GetOrAddComponent<CanvasGroup>();

            if (ForceStartValues)
            {
                ret.Append(cg.DOFade(end_alpha, 0.0f));
            }

            ret.Append(cg.DOFade(start_alpha, time).SetEase(Ease.InOutQuad));

            return ret;
        }
    }
}
