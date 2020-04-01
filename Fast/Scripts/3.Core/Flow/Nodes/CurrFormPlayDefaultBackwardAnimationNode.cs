using System;
using UnityEngine;

public static class CurrFormPlayDefaultBackwardAnimationExtensions
{
    public static Fast.Flow.FlowContainer FlowCurrFormPlayDefaultBackwardAnimation(this Fast.Flow.FlowContainer container,
        bool force_starting_values = false)
    {
        Fast.Flow.CurrFormPlayDefaultBackwardAnimationNode node = new Fast.Flow.CurrFormPlayDefaultBackwardAnimationNode(container,
            force_starting_values);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class CurrFormPlayDefaultBackwardAnimationNode : FlowNode
    {
        private bool force_starting_values = false;

        public CurrFormPlayDefaultBackwardAnimationNode(FlowContainer container,
             bool force_starting_values)
            : base(container)
        {
            this.force_starting_values = force_starting_values;
        }

        protected override void OnRunInternal()
        {
            if (Container.Controller.FlowState.CurrForm != null)
            {
                UI.FormAnimation curr_animation =
                    Container.Controller.FlowState.CurrForm.DefaultAnimation;

                if (curr_animation != null)
                {
                    curr_animation.ForceStartingValues = force_starting_values;

                    curr_animation.OnFinish.Subscribe(delegate ()
                    {
                        Finish();
                    });

                    //curr_animation.AnimateBackward();
                }
                else
                {
                    Debug.LogError("[Fast.Flow.CurrFormPlayBackwardAnimationNode] The selected animation type could not be found");
                }
            }
            else
            {
                Finish();
            }
        }
    }
}