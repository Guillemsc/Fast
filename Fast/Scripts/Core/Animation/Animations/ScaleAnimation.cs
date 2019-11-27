using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Fast.Animations
{
    class ScaleAnimation : Animation
    {
        private GameObject go = null;
        private float time = 0.0f;

        private Vector3 start_scale = Vector3.zero;
        private Vector3 end_scale = Vector3.zero;

        public ScaleAnimation(GameObject go, float time, Vector3 start_scale, Vector3 end_scale,
            bool force_start_value = false) : base(force_start_value)
        {
            this.go = go;
            this.time = time;
            this.start_scale = start_scale;
            this.end_scale = end_scale;
        }

        public override Sequence AnimateForward()
        {
            Sequence ret = DOTween.Sequence();

            if (ForceStartValues)
            {
                ret.Append(go.transform.DOScale(start_scale, 0.0f));
            }

            ret.Append(go.transform.DOScale(end_scale, time).SetEase(Ease.InOutQuad));

            return ret;
        }

        public override Sequence AnimateBackward()
        {
            Sequence ret = DOTween.Sequence();

            if (ForceStartValues)
            {
                ret.Append(go.transform.DOScale(end_scale, 0.0f));
            }

            ret.Append(go.transform.DOScale(start_scale, time).SetEase(Ease.InOutQuad));

            return ret;
        }
    }
}