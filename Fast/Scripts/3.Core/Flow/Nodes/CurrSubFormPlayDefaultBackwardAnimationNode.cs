using System;
using UnityEngine;

public static class CurrSubFormPlayDefaultBackwardAnimationExtensions
{
    public static Fast.Flow.FlowContainer FlowCurrSubFormPlayDefaultBackwardAnimation(this Fast.Flow.FlowContainer container,
        bool force_starting_values = false)
    {
        Fast.Flow.CurrSubFormPlayDefaultBackwardAnimationNode node = new Fast.Flow.CurrSubFormPlayDefaultBackwardAnimationNode(container,
            force_starting_values);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class CurrSubFormPlayDefaultBackwardAnimationNode : FlowNode
    {
        private bool force_starting_values = false;

        public CurrSubFormPlayDefaultBackwardAnimationNode(FlowContainer container,
             bool force_starting_values)
            : base(container)
        {
            this.force_starting_values = force_starting_values;
        }

        protected override void OnRunInternal()
        {
            if (Container.Controller.FlowState.CurrSubForm != null)
            {
                if (Container.Controller.FlowState.CurrSubForm.DefaultAnimation != null)
                {
                    Container.Controller.FlowState.CurrSubForm.DefaultAnimation.ForceStartingValues = force_starting_values;

                    Container.Controller.FlowState.CurrSubForm.DefaultAnimation.OnFinish.Subscribe(delegate ()
                    {
                        Finish();
                    });

                    //Container.Controller.FlowState.CurrSubForm.DefaultAnimation.AnimateBackward();
                }
                else
                {
                    Debug.LogError("[Fast.Flow.CurrSubFormPlayDefaultBackwardAnimationNode] The selected animation type could not be found");
                }
            }
            else
            {
                Finish();
            }
        }
    }
}