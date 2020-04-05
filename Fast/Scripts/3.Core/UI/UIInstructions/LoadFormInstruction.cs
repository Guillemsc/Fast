using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.UI
{
    public class LoadFormInstruction : Fast.UI.UIInstruction
    {
        private readonly string prefab_scene_name = "";
        private readonly Fast.Scenes.LoadedScene scene_to_set = null;
        private readonly GameObject parent = null;

        public LoadFormInstruction(string prefab_scene_name, Fast.Scenes.LoadedScene scene_to_set, GameObject parent)
        {
            this.prefab_scene_name = prefab_scene_name;
            this.scene_to_set = scene_to_set;
            this.parent = parent;
        }

        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            Scenes.Scene loadable_scene = Fast.FastService.MScenes.GetLoadableScene(prefab_scene_name);

            if(loadable_scene == null)
            {
                Finish();

                return;
            }

            Fast.FastService.MPrefabScenes.LoadPrefabSceneAsync<Fast.UI.Form>(loadable_scene, scene_to_set, parent).ContinueWith(
            delegate(Task<Fast.PrefabScenes.PrefabScene<Fast.UI.Form>> task)
            {
                context.LastLoadedForm = task.Result;

                Finish();
            });
        }
    }
}
