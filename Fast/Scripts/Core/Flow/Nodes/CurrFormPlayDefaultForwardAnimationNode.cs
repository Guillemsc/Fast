using System;
using UnityEngine;

public static class CurrFormPlayDefaultForwardAnimationExtensions
{
    public static Fast.Flow.FlowContainer FlowCurrFormPlayDefaultForwardAnimation(this Fast.Flow.FlowContainer container,
        bool force_starting_values = false)
    {
        Fast.Flow.CurrFormPlayDefaultForwardAnimationNode node = new Fast.Flow.CurrFormPlayDefaultForwardAnimationNode(container,
            force_starting_values);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class CurrFormPlayDefaultForwardAnimationNode : FlowNode
    {
        private bool force_starting_values = false;

        public CurrFormPlayDefaultForwardAnimationNode(FlowContainer container,
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

                    curr_animation.AnimateForward();
                }
                else
                {
                    Debug.LogError("[Fast.Flow.CurrFormPlayDefaultForwardAnimationNode] The selected animation type could not be found");
                }
            }
            else
            {
                Finish();
            }
        }
    }
}