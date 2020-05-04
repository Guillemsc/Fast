using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Fast.Modules
{
    public class ScenesModule : Module
    {
        private readonly Fast.Scenes.SceneController controller = new Scenes.SceneController();

        private Fast.Scenes.ScenesConfigAsset scenes_config_asset = null;

        public void SetScenesConfig(Scenes.ScenesConfigAsset scenes_config_asset)
        {
            this.scenes_config_asset = scenes_config_asset;

            controller.SetConfig(scenes_config_asset);
        }

        public async Task<bool> LoadSceneAsync(Fast.Scenes.SceneReference reference)
        {
            return await controller.LoadSceneAsync(reference);
        }

        public async Task UnloadSceneAsync(Fast.Scenes.SceneReference reference)
        {
            await controller.UnloadSceneAsync(reference);
        }
    }
}
