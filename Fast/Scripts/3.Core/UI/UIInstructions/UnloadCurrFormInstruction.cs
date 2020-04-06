using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.UI
{
    public class UnloadCurrFormInstruction : Fast.UI.UIInstruction
    {
        public UnloadCurrFormInstruction()
        {

        }

        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            Fast.PrefabScenes.PrefabScene<Fast.UI.Form> form_prefab_scene = context.Controller.CurrForm;

            if (form_prefab_scene == null)
            {
                Finish();

                return;
            }

            Fast.FastService.MPrefabScenes.UnloadPrefabScene(form_prefab_scene).ContinueWith(
            delegate (Task t)
            {
                Finish();
            });

            if (form_prefab_scene == context.Controller.CurrForm)
            {
                context.Controller.CurrForm = null;
            }
        }
    }
}
