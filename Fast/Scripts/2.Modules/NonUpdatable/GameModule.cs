using System;

namespace Fast.Modules
{
    public class GameModule : Module
    {
        private Game.GameConfigAsset game_config_asset = null;

        public void SetGameConfig(Game.GameConfigAsset game_config_asset)
        {
            this.game_config_asset = game_config_asset;
        }
    }
}
