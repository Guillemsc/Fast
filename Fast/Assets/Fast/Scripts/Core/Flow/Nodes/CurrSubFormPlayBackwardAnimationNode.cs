using System;
using UnityEngine;

public static class CurrSubFormPlayBackwardAnimationExtensions
{
    public static Fast.Flow.FlowContainer FlowCurrSubFormPlayBackwardAnimation(this Fast.Flow.FlowContainer container,
        string animation_name, bool force_starting_values = false)
    {
        Fast.Flow.CurrSubFormPlayBackwardAnimationNode node = new Fast.Flow.CurrSubFormPlayBackwardAnimationNode(container,
            animation_name, force_starting_values);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class CurrSubFormPlayBackwardAnimationNode : FlowNode
    {
        private string animation_name = "";
        private bool force_starting_values = false;

        public CurrSubFormPlayBackwardAnimationNode(FlowContainer container, string animation_name,
             bool force_starting_values)
            : base(container)
        {
            this.animation_name = animation_name;
            this.force_starting_values = force_starting_values;
        }

        protected override void OnRunInternal()
        {
            if (Container.Controller.FlowState.CurrSubForm != null)
            {
                UI.FormAnimation curr_animation =
                    Container.Controller.FlowState.CurrSubForm.GetAnimation(animation_name);

                if (curr_animation != null)
                {
                    curr_animation.ForceStartingValues = force_starting_values;

                    curr_animation.OnFinish.Subscribe(delegate ()
                    {
                        Finish();
                    });

                    curr_animation.AnimateBackward();
                }
                else
                {
                    Debug.LogError("[Fast.Flow.CurrSubFormPlayBackwardAnimationNode] The selected animation type could not be found");
                }
            }
            else
            {
                Finish();
            }
        }
    }
}