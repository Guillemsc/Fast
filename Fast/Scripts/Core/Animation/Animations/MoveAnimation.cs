﻿using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Fast.Animations
{
    class MoveAnimation : Animation
    {
        private GameObject go = null;
        private float time = 0.0f;

        private Vector3 start_pos = Vector3.zero;
        private Vector3 end_pos = Vector3.zero;

        public MoveAnimation(GameObject go, float time, Vector3 start_pos, Vector3 end_pos, 
            bool force_start_value = false) : base(force_start_value)
        {
            this.go = go;
            this.time = time;
            this.start_pos = start_pos;
            this.end_pos = end_pos;
        }

        public override Sequence AnimateForward()
        {
            Sequence ret = DOTween.Sequence();

            if (ForceStartValues)
            {
                ret.Append(go.transform.DOMove(start_pos, 0.0f));
            }

            ret.Append(go.transform.DOMove(end_pos, time).SetEase(Ease.InOutQuad));

            return ret;
        }

        public override Sequence AnimateBackward()
        {
            Sequence ret = DOTween.Sequence();

            if (ForceStartValues)
            {
                ret.Append(go.transform.DOMove(end_pos, 0.0f));
            }

            ret.Append(go.transform.DOMove(start_pos, time).SetEase(Ease.InOutQuad));

            return ret;
        }
    }
}