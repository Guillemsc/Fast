using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public class ScaleTriggerBinding : DOTweenMultipleTargetBinding<GameObject>
    {
        [SerializeField] private Vector2 start_scale = Vector2.zero;
        [SerializeField] private Vector2 end_scale = Vector2.one;

        ScaleTriggerBinding() : base(true)
        {

        }

        protected override void SetupTarget(Sequence seq, GameObject target, object value)
        {
            if (UseStartingValue)
            {
                seq.Append(target.transform.DOScale(start_scale, 0.0f));
            }

            Tween tween = target.transform.DOScale(end_scale, Duration);
            SetEasing(tween);

            seq.Append(tween);
        }
    }
}
