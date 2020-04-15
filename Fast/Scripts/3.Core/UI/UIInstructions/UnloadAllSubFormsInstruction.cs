using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.UI
{
    public class UnloadAllSubFormsInstruction : Fast.UI.UIInstruction
    {
        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            UnloadAll(context.Controller.CurrSubForms).ContinueWith(
            delegate (Task t)
            {
                context.Controller.CurrForm = null;
                context.Controller.CurrSubForms.Clear();

                Finish();
            });
        }

        public async Task UnloadAll(List<Fast.PrefabScenes.PrefabScene<Fast.UI.Form>> sub_forms)
        {
            for (int i = 0; i < sub_forms.Count; ++i)
            {
                Fast.PrefabScenes.PrefabScene<Fast.UI.Form> form_prefab = sub_forms[i];

                await Fast.FastService.MPrefabScenes.UnloadPrefabSceneAsync(form_prefab);
            }
        }
    }
}
