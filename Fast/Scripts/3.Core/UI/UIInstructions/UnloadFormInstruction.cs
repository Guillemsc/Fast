using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.UI
{
    public class UnloadFormInstruction : Fast.UI.UIInstruction
    {
        private readonly Fast.PrefabScenes.PrefabSceneReference<Fast.UI.Form> reference = null;

        public UnloadFormInstruction(Fast.PrefabScenes.PrefabSceneReference<Fast.UI.Form> reference)
        {
            this.reference = reference;
        }

        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            Fast.PrefabScenes.PrefabScene<Fast.UI.Form> form_prefab_scene = Fast.FastService.MPrefabScenes.
                GetLoadedPrefabScene<Fast.UI.Form>(reference);

            if(form_prefab_scene == null)
            {
                Finish();

                return;
            }

            Fast.FastService.MPrefabScenes.UnloadPrefabSceneAsync(reference).ContinueWith(
            delegate(Task t)
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
