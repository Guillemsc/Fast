using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Fast.UI.Bindings
{
    public class PositionTriggerBinding : DOTweenMultipleTargetBinding<GameObject>
    {
        [SerializeField] private Fast.Others.CoordinateSpace space = Others.CoordinateSpace.WORLD;
        [SerializeField] private Vector2 start_position = Vector2.zero;
        [SerializeField] private Vector2 end_position = Vector2.one;

        PositionTriggerBinding() : base(true)
        {

        }

        protected override void SetupTarget(Sequence seq, GameObject target, object value)
        {
            if (UseStartingValue)
            {
                switch (space)
                {
                    case Others.CoordinateSpace.WORLD:
                        {
                            seq.Append(target.transform.DOMove(start_position, 0.0f));
                            break;
                        }

                    case Others.CoordinateSpace.LOCAL:
                        {
                            seq.Append(target.transform.DOLocalMove(start_position, 0.0f));
                            break;
                        }
                }
            }

            Tween tween = null;

            switch (space)
            {
                case Others.CoordinateSpace.WORLD:
                    {
                        tween = target.transform.DOMove(end_position, Duration);
                        break;
                    }

                case Others.CoordinateSpace.LOCAL:
                    {
                        tween = target.transform.DOLocalMove(end_position, Duration);
                        break;
                    }
            }

            SetEasing(tween);

            seq.Append(tween);
        }
    }
}
