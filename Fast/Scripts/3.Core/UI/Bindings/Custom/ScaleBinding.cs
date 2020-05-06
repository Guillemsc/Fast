using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public class ScaleBinding : DOTweenBinding
    {
        [SerializeField] private Vector2 start_scale = Vector2.zero;
        [SerializeField] private Vector2 end_scale = Vector2.one;

        protected override void SetupSequence(Sequence seq, GameObject target)
        {
            if (UseStartingValue)
            {
                seq.Append(target.transform.DOScale(start_scale, 0.0f));
            }

            seq.Append(target.transform.DOScale(end_scale, Duration));
        }
    }
}
