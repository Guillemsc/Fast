using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.UI
{
    public class UnloadCurrSubFormInstruction : Fast.UI.UIInstruction
    {
        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            Fast.PrefabScenes.PrefabScene<Fast.UI.Form> form_prefab_scene = context.Controller.CurrSubForm;

            if (form_prefab_scene == null)
            {
                Finish();

                return;
            }

            Fast.FastService.MPrefabScenes.UnloadPrefabSceneAsync(form_prefab_scene).ContinueWith(
            delegate (Task t)
            {
                Finish();
            });

            if (form_prefab_scene == context.Controller.CurrSubForm)
            {
                context.Controller.CurrSubForm = null;
                context.Controller.CurrSubForms.Remove(form_prefab_scene);
            }
        }
    }
}
