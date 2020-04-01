using System;
using UnityEngine;

public static class AllSubFormsPlayDefaultBackwardAnimationExtensions
{
    public static Fast.Flow.FlowContainer FlowAllSubFormsPlayDefaultBackwardAnimation(this Fast.Flow.FlowContainer container,
        bool force_starting_values = false)
    {
        Fast.Flow.AllSubFormsPlayDefaultBackwardAnimationNode node = new Fast.Flow.AllSubFormsPlayDefaultBackwardAnimationNode(container,
            force_starting_values);

        container.AddFlowNode(node);

        return container;
    }
}

namespace Fast.Flow
{
    public class AllSubFormsPlayDefaultBackwardAnimationNode : FlowNode
    {
        private bool force_starting_values = false;

        private int animations_to_finish = 0;
        private int animations_completed = 0;

        public AllSubFormsPlayDefaultBackwardAnimationNode(FlowContainer container,
             bool force_starting_values)
            : base(container)
        {
            this.force_starting_values = force_starting_values;
        }

        protected override void OnRunInternal()
        {
            animations_to_finish = Container.Controller.FlowState.CurrSubForms.Count;

            if (animations_to_finish > 0)
            {
                for (int i = 0; i < Container.Controller.FlowState.CurrSubForms.Count; ++i)
                {
                    Fast.UI.Form curr_form = Container.Controller.FlowState.CurrSubForms[i];

                    if (curr_form.DefaultAnimation != null)
                    {
                        curr_form.DefaultAnimation.ForceStartingValues = force_starting_values;

                        curr_form.DefaultAnimation.OnFinish.Subscribe(delegate ()
                        {
                            ++animations_completed;

                            if (animations_completed >= animations_to_finish)
                            {
                                Finish();
                            }
                        });

                        //curr_form.DefaultAnimation.AnimateBackward();
                    }
                    else
                    {
                        Debug.LogWarning("[Fast.Flow.AllSubFormsPlayDefaultBackwardAnimationNode] " +
                            "The selected animation type could not be found");

                        ++animations_completed;

                        if (animations_completed >= animations_to_finish)
                        {
                            Finish();
                        }
                    }
                }
            }
            else
            {
                Finish(); 
            }
        }
    }
}