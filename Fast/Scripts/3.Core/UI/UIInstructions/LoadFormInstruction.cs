using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.UI
{
    public class LoadFormInstruction : Fast.UI.UIInstruction
    {
        private readonly string prefab_scene_name = "";

        public LoadFormInstruction(string prefab_scene_name)
        {
            this.prefab_scene_name = prefab_scene_name;
        }

        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            Scenes.Scene loadable_scene = Fast.FastService.MScenes.GetLoadableScene(prefab_scene_name);

            if(loadable_scene == null)
            {
                Finish();

                return;
            }

            Fast.FastService.MPrefabScenes.LoadPrefabSceneAsync<Fast.UI.Form>(loadable_scene).ContinueWith(
            delegate(Task<Fast.PrefabScenes.PrefabScene<Fast.UI.Form>> task)
            {
                context.LastLoadedForm = task.Result;

                Finish();
            });
        }
    }
}
