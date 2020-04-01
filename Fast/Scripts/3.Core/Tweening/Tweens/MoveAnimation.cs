using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Fast.Tweening
{
    class MoveTween : Tween
    {
        private GameObject go = null;
        private float time = 0.0f;

        private Vector3 start_pos = Vector3.zero;
        private Vector3 end_pos = Vector3.zero;

        private Ease ease = Ease.Linear;

        public MoveTween(GameObject go, Vector3 start_pos, Vector3 end_pos, float time = 1.0f,
            bool force_start_value = false) : base(force_start_value)
        {
            this.go = go;
            this.start_pos = start_pos;
            this.end_pos = end_pos;
            this.time = time;
        }

        public void SetEase(Ease ease)
        {
            this.ease = ease;
        }

        public void SetTime(float time)
        {
            this.time = time;
        }

        public override void SetStartingValuesForward()
        {
            go.transform.localPosition = start_pos;
        }

        public override void SetStartingValuesBackward()
        {
            go.transform.localPosition = end_pos;
        }

        public override Sequence AnimateForward()
        {
            Sequence ret = DOTween.Sequence();

            CanvasGroup cg = go.GetOrAddComponent<CanvasGroup>();

            if (ForceStartValues)
            {
                ret.AppendCallback(SetStartingValuesForward);
            }

            ret.Append(go.transform.DOLocalMove(end_pos, time).SetEase(ease));

            return ret;
        }

        public override Sequence AnimateBackward()
        {
            Sequence ret = DOTween.Sequence();

            CanvasGroup cg = go.GetOrAddComponent<CanvasGroup>();

            if (ForceStartValues)
            {
                ret.AppendCallback(SetStartingValuesBackward);
            }

            ret.Append(go.transform.DOLocalMove(start_pos, time).SetEase(ease));

            return ret;
        }
    }
}