//using System;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Fast.UI.Bindings
//{
//    class ImageBinding : MultipleTargetsBinding<Image>
//    {
//        public override void OnValueRised(object value)
//        {
//            Sprite sprite = value as Sprite;

//            if(sprite == null)
//            {
//                return;
//            }

//            for(int i = 0; i < Targets.Count; ++i)
//            {
//                Image curr_target = Targets[i];

//                if(curr_target == null)
//                {
//                    continue;
//                }

//                curr_target.sprite = sprite;
//            }
//        }
//    }
//}
