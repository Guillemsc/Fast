using System;
using UnityEngine;

public static class CurrSubFormPlayDefaultForwardAnimationExtensions
{
    public static Fast.Flow.FlowContainer FlowCurrSubFormPlayDefaultForwardAnimation(this Fast.Flow.FlowContainer container,
        bool force_starting_values = false)
    {
        Fast.Flow.CurrSubFormPlayDefaultForwardAnimationNode node = new Fast.Flow.CurrSubFormPlayDefaultForwardAnimationNode(container,
            force_starting_values);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class CurrSubFormPlayDefaultForwardAnimationNode : FlowNode
    {
        private bool force_starting_values = false;

        public CurrSubFormPlayDefaultForwardAnimationNode(FlowContainer container,
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

                    //Container.Controller.FlowState.CurrSubForm.DefaultAnimation.AnimateForward();
                }
                else
                {
                    Debug.LogError("[Fast.Flow.CurrSubFormPlayDefaultForwardAnimationNode] The selected animation type could not be found");
                }
            }
            else
            {
                Finish();
            }
        }
    }
}