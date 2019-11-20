using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class BasicFormTransitionPremade
{
    public static Fast.Flow.FlowNode PremadeBasicFormTransition(this Fast.Flow.FlowContainer container,
        string to_deactivate_transition, bool to_deactivate_transition_backward, Fast.UI.Form to_activate,
        string to_activate_transition, bool to_activate_transition_backward)
    {
        Fast.Flow.FlowNode ret = null;

        container.FlowAllSubFormsPlayDefaultBackwardAnimation();

        if (to_deactivate_transition_backward)
        {
            container.FlowNextStartWithLast().FlowCurrFormPlayBackwardAnimation(to_deactivate_transition, false);
        }
        else
        {
            container.FlowNextStartWithLast().FlowCurrFormPlayForwardAnimation(to_deactivate_transition, false);
        }

        container.FlowNextStartWithLast().FlowSetCurrForm(to_activate);

        ret = container.LastNode;

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
            .FlowCurrFormSetActive(false)

            .FlowSetCurrForm(to_activate)
            .FlowAllSubFormsHide()
            .FlowAllSubFormsSetActive(false)
            .FlowRemoveAllSubForms();

        return ret;
    }

    public static void PremadeOpenSubFormTransition(this Fast.Flow.FlowContainer container,
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
    }

    public static void PremadeCloseSubFormTransition(this Fast.Flow.FlowContainer container,
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
    }
}

