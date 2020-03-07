using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Fast.Animations
{
    class RotateAnimation : Animation
    {
        private GameObject go = null;
        private float time = 0.0f;

        private Vector3 start_rotation = Vector3.zero;
        private Vector3 end_rotation = Vector3.zero;

        public RotateAnimation(GameObject go, Vector3 start_rotation, Vector3 end_rotation, float time = 1.0f,
            bool force_start_value = false) : base(force_start_value)
        {
            this.go = go;
            this.time = time;
            this.start_rotation = start_rotation;
            this.end_rotation = end_rotation;
        }

        public override Sequence AnimateForward()
        {
            Sequence ret = DOTween.Sequence();

            CanvasGroup cg = go.GetOrAddComponent<CanvasGroup>();

            if (ForceStartValues)
            {
                ret.Append(go.transform.DOLocalRotate(start_rotation, 0.0f));
            }

            ret.AppendCallback(delegate ()
            {
                cg.interactable = false;
            });

            ret.Append(go.transform.DOLocalRotate(end_rotation, time).SetEase(Ease.InOutQuad));

            ret.AppendCallback(delegate ()
            {
                cg.interactable = true;
            });

            return ret;
        }

        public override Sequence AnimateBackward()
        {
            Sequence ret = DOTween.Sequence();

            CanvasGroup cg = go.GetOrAddComponent<CanvasGroup>();

            if (ForceStartValues)
            {
                ret.Append(go.transform.DOLocalRotate(end_rotation, 0.0f));
            }

            ret.Append(go.transform.DOLocalRotate(start_rotation, time).SetEase(Ease.InOutQuad));

            return ret;
        }
    }
}