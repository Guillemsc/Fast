using System;

namespace Fast.Saving
{
    public class SettingsSaveController : Fast.IController, IUpdatable
    {
        private bool wants_to_save = false;
        private bool wants_to_load = false;

        private SettingsSaveData last_loaded_file = null;

        private readonly Fast.Callback<SettingsSaveData> on_load_data = new Callback<SettingsSaveData>();
        private readonly Fast.Callback<SettingsSaveData> on_save_data = new Callback<SettingsSaveData>();

        public Fast.Callback<SettingsSaveData> OnLoadData => on_load_data;
        public Fast.Callback<SettingsSaveData> OnSaveData => on_save_data;

        public void Update()
        {
            CheckSave();
            CheckLoad();
        }

        public void Save()
        {
            wants_to_save = true;
        }

        public void Load()
        {
            wants_to_load = true;
        }

        private void CheckSave()
        {
            if(!wants_to_save)
            {
                return;
            }

            wants_to_save = false;

            ActuallySave();
        }

        private void CheckLoad()
        {
            if (!wants_to_load)
            {
                return;
            }

            wants_to_load = false;

            ActuallySave();
        }

        private void ActuallyLoad()
        {
            string filepath = $"{Fast.FastService.MApplication.PersistentDataPath}Fast/SettingsSaveData/data.save";

            SettingsSaveData loaded_file = null;
            bool success = Fast.Serializers.JSONSerializer.DeSerializeFromPath(filepath, out loaded_file);

            if(!success)
            {
                return;
            }

            on_load_data.Invoke(loaded_file);
        }

        private void ActuallySave()
        {
            string filepath = $"{Fast.FastService.MApplication.PersistentDataPath}Fast/SettingsSaveData/data.save";

            SettingsSaveData loaded_file = last_loaded_file;

            if (loaded_file == null)
            {
                loaded_file = new SettingsSaveData();
            }

            on_save_data.Invoke(loaded_file);

            Fast.Serializers.JSONSerializer.SerializeToPath(filepath, loaded_file);
        }
    }
}
