using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public class TMProBinding : DOTweenMultipleTargetBinding<TextMeshProUGUI>
    {
        [SerializeField] private bool rich_text_enabled = true;
        [SerializeField] private ScrambleMode scramble_mode = ScrambleMode.None;

        TMProBinding() : base(false)
        {

        }

        protected override void SetupTarget(Sequence seq, TextMeshProUGUI target, object value)
        {
            string string_value = value.ToString();

            seq.Append(target.DOText("", 0.0f, rich_text_enabled, scramble_mode));

            Tween tween = target.DOText(string_value, Duration, rich_text_enabled, scramble_mode);
            SetEasing(tween);

            seq.Append(tween);
        }
    }
}
