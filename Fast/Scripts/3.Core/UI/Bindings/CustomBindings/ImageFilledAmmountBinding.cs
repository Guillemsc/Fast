using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Fast.UI.Bindings
{
    public class ImageFilledAmmountBinding : DOTweenMultipleTargetBinding<Image>
    {
        ImageFilledAmmountBinding() : base(false)
        {

        }

        protected override void SetupTarget(Sequence seq, Image target, object value)
        {
            float string_value = (float)value;

            Tween tween = target.DOFillAmount(string_value, Duration);
            SetEasing(tween);

            seq.Append(tween);
        }
    }
}
