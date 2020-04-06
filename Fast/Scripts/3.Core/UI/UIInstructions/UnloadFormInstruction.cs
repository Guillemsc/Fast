using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.UI
{
    public class UnloadFormInstruction : Fast.UI.UIInstruction
    {
        private readonly string prefab_scene_name = "";

        public UnloadFormInstruction(string prefab_scene_name)
        {
            this.prefab_scene_name = prefab_scene_name;
        }

        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            Fast.PrefabScenes.BasePrefabScene form_prefab_scene = Fast.FastService.MPrefabScenes.GetLoadedPrefabScene(prefab_scene_name);

            if(form_prefab_scene == null)
            {
                Finish();

                return;
            }

            Fast.FastService.MPrefabScenes.UnloadPrefabScene(form_prefab_scene).ContinueWith(
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
