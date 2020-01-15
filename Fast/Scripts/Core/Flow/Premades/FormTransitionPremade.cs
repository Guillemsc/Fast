using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class FormTransitionPremade
{
    private static Fast.Flow.FlowContainer PremadeFormTransitionEx(this Fast.Flow.FlowContainer container,
        string to_deactivate_transition, bool to_deactivate_transition_backward, bool transiton_start_together, Fast.UI.Form to_activate,
        string to_activate_transition, bool to_activate_transition_backward, out Fast.Flow.FlowNode mid_node)
    {
        container.FlowAllSubFormsPlayDefaultBackwardAnimation();

        if (to_deactivate_transition_backward)
        {
            container.FlowNextStartWithLast().FlowCurrFormPlayBackwardAnimation(to_deactivate_transition, false);
        }
        else
        {
            container.FlowNextStartWithLast().FlowCurrFormPlayForwardAnimation(to_deactivate_transition, false);
        }

        if (transiton_start_together)
        {
            container.FlowNextStartWithLast().FlowSetCurrForm(to_activate);
        }
        else
        {
            container.FlowSetCurrForm(to_activate);
        }

        mid_node = container.LastNode;

        if (to_activate_transition_backward)
        {
            container.FlowNextStartWithLast().FlowCurrFormPlayBackwardAnimation(to_activate_transition, true);
        }
        else
        {
            container.FlowNextStartWithLast().FlowCurrFormPlayForwardAnimation(to_activate_transition, true);
        }

        container
            .FlowNextStartWithLast().FlowCurrFormSetActive(true)
            .FlowNextStartWithLast().FlowCurrFormShow()

            .FlowLastCurrFormSetAsCurrForm()
            .FlowCurrFormHide()
            .FlowNextStartWithLast().FlowCurrFormSetActive(false)

            .FlowSetCurrForm(to_activate)
            .FlowNextStartWithLast().FlowAllSubFormsHide()
            .FlowNextStartWithLast().FlowAllSubFormsSetActive(false)
            .FlowNextStartWithLast().FlowRemoveAllSubForms();

        return container;
    }

    public static Fast.Flow.FlowContainer PremadeFormTransition(this Fast.Flow.FlowContainer container,
        string to_deactivate_transition, bool to_deactivate_transition_backward, Fast.UI.Form to_activate,
        string to_activate_transition, bool to_activate_transition_backward)
    {
        Fast.Flow.FlowNode out_node;
        return PremadeFormTransitionEx(container, to_deactivate_transition, to_deactivate_transition_backward, false,
                                        to_activate, to_activate_transition, to_activate_transition_backward, out out_node);
    }

    public static Fast.Flow.FlowContainer PremadeFormTransition(this Fast.Flow.FlowContainer container,
        string to_deactivate_transition, bool to_deactivate_transition_backward, Fast.UI.Form to_activate,
        string to_activate_transition, bool to_activate_transition_backward, out Fast.Flow.FlowNode mid_node)
    {
        return PremadeFormTransitionEx(container, to_deactivate_transition, to_deactivate_transition_backward, false,
                                        to_activate, to_activate_transition, to_activate_transition_backward, out mid_node);
    }

    public static Fast.Flow.FlowContainer PremadeFormTransitionTogether(this Fast.Flow.FlowContainer container,
        string to_deactivate_transition, bool to_deactivate_transition_backward, Fast.UI.Form to_activate,
        string to_activate_transition, bool to_activate_transition_backward)
    {
        Fast.Flow.FlowNode out_node;
        return PremadeFormTransitionEx(container, to_deactivate_transition, to_deactivate_transition_backward, true,
                                        to_activate, to_activate_transition, to_activate_transition_backward, out out_node);
    }

    public static Fast.Flow.FlowContainer PremadeFormTransitionTogether(this Fast.Flow.FlowContainer container,
        string to_deactivate_transition, bool to_deactivate_transition_backward, Fast.UI.Form to_activate,
        string to_activate_transition, bool to_activate_transition_backward, out Fast.Flow.FlowNode mid_node)
    {
        return PremadeFormTransitionEx(container, to_deactivate_transition, to_deactivate_transition_backward, true,
                                        to_activate, to_activate_transition, to_activate_transition_backward, out mid_node);
    }

    public static Fast.Flow.FlowContainer PremadeShowForm(this Fast.Flow.FlowContainer container,
        Fast.UI.Form to_activate)
    {
        container
            .FlowNextStartWithLast().FlowAllSubFormsHide()
            .FlowNextStartWithLast().FlowAllSubFormsSetActive(false)
            .FlowNextStartWithLast().FlowRemoveAllSubForms()
            .FlowNextStartWithLast().FlowCurrFormHide()
            .FlowNextStartWithLast().FlowCurrFormSetActive(false)
            .FlowSetCurrForm(to_activate)
            .FlowNextStartWithLast().FlowCurrFormShow()
            .FlowNextStartWithLast().FlowCurrFormSetActive(true);

        return container;
    }

    public static Fast.Flow.FlowContainer PremadeOpenSubFormTransition(this Fast.Flow.FlowContainer container,
        Fast.UI.Form to_activate, string to_activate_animation, bool to_activate_animation_backward)
    {
        container.FlowAddAndSetCurrSubForm(to_activate);

        if(to_activate_animation_backward)
        {
            container.FlowCurrSubFormPlayBackwardAnimation(to_activate_animation, true);
        }
        else
        {
            container.FlowCurrSubFormPlayForwardAnimation(to_activate_animation, true);
        }

        container
            .FlowNextStartWithLast().FlowCurrSubFormSetActive(true)
            .FlowNextStartWithLast().FlowCurrSubFormShow();

        return container;
    }

    public static Fast.Flow.FlowContainer PremadeCloseSubFormTransition(this Fast.Flow.FlowContainer container,
        Fast.UI.Form to_activate, string to_activate_animation, bool to_activate_animation_backward)
    {
        container.FlowAddAndSetCurrSubForm(to_activate);

        if (to_activate_animation_backward)
        {
            container.FlowCurrSubFormPlayBackwardAnimation(to_activate_animation);
        }
        else
        {
            container.FlowCurrSubFormPlayForwardAnimation(to_activate_animation);
        }

        container
            .FlowCurrSubFormSetActive(false)
            .FlowNextStartWithLast().FlowCurrSubFormHide()
            .FlowRemoveCurrSubForm();

        return container;
    }
}

