using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Modules
{
    public class SettingsDataSaveModule : Fast.Modules.UpdatableModule
    {
        private readonly Fast.Saving.SettingsSaveController controller = new Saving.SettingsSaveController();

        public override void PostUpdate()
        {
            controller.Update();
        }

        public Fast.Callback<Fast.Saving.SettingsSaveData> OnLoadData => controller.OnLoadData;
        public Fast.Callback<Fast.Saving.SettingsSaveData> OnSaveData => controller.OnSaveData;

        public void Save()
        {
            controller.Save();
        }

        public void Load()
        {
            controller.Load();
        }
    }
}
