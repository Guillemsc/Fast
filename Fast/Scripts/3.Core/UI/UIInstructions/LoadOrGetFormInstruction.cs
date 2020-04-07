using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.UI
{
    public class LoadOrGetFormInstruction : Fast.UI.UIInstruction
    {
        private readonly Fast.PrefabScenes.PrefabSceneReference<Fast.UI.Form> form_reference = null;

        public LoadOrGetFormInstruction(Fast.PrefabScenes.PrefabSceneReference<Fast.UI.Form> form_reference)
        {
            this.form_reference = form_reference;
        }

        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            if(form_reference == null)
            {
                Finish();

                return;
            }

            Fast.FastService.MPrefabScenes.LoadPrefabSceneAsync(form_reference).ContinueWith(
            delegate(Task<Fast.PrefabScenes.PrefabScene<Fast.UI.Form>> task)
            {
                context.LastLoadedForm = task.Result;

                Finish();
            });
        }
    }
}
