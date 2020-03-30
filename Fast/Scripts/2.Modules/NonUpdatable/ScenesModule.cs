using System;

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
    }
}
