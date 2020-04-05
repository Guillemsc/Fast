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

            controller.SetLoadableScenes(scenes_config_asset.Scenes);
        }

        public override void Awake()
        {
            controller.Start();
        }

        public Fast.Scenes.LoadedScene RootScene => controller.RootScene;

        public Fast.Scenes.Scene GetLoadableScene(string scene_name)
        {
            return controller.GetLoadableScene(scene_name);
        }

        public async Task<Fast.Scenes.LoadedScene> LoadSceneAsync(Fast.Scenes.Scene scene, LoadSceneMode mode)
        {
            return await controller.LoadSceneAsync(scene, mode);
        }

        public async Task UnloadSceneAsync(Fast.Scenes.Scene scene)
        {
            await controller.UnloadSceneAsync(scene);
        }
    }
}
