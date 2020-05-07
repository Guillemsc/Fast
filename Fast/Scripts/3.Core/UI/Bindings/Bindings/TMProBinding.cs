//using System;
//using DG.Tweening;
//using TMPro;
//using UnityEngine;

//namespace Fast.UI.Bindings
//{
//    public class TMProBinding : DOTweenBinding<TextMeshProUGUI>
//    {
//        [SerializeField] private bool rich_text_enabled = true;
//        [SerializeField] private ScrambleMode scramble_mode = ScrambleMode.All;

//        protected override void SetupSequence(Sequence seq, TextMeshProUGUI target, object value)
//        {
//            string string_value = value.ToString();

//            seq.Append(target.DOText(string_value, Duration, rich_text_enabled, scramble_mode));
//        }
//    }
//}
