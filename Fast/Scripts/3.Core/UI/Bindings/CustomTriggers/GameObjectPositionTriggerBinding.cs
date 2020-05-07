using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public class GameObjectPositionTriggerBinding : DOTweenMultipleTargetBinding<GameObject>
    {
        [SerializeField] private GameObject start_position = null;
        [SerializeField] private GameObject end_position = null;

        GameObjectPositionTriggerBinding() : base(true)
        {

        }

        protected override void SetupTarget(Sequence seq, GameObject target, object value)
        {
            if (UseStartingValue)
            {
                if(start_position != null)
                {
                    seq.Append(target.transform.DOMove(start_position.transform.position, 0.0f));
                }
            }

            if (end_position != null)
            {
                Tween tween = target.transform.DOMove(end_position.transform.position, Duration);
                SetEasing(tween);

                seq.Append(tween);
            }
        }
    }
}
