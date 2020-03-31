using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Fast.Saving
{
    public class GameDataSaveController : Fast.IController
    {
        private readonly List<GameDataSaveSlot> loaded_slots = new List<GameDataSaveSlot>();

        private GameDataSaveSlot current_slot = null;

        private readonly Fast.Callback<GameDataSaveSlot> on_load_current_slot = new Callback<GameDataSaveSlot>();
        private readonly Fast.Callback<GameDataSaveSlot> on_save_current_slot = new Callback<GameDataSaveSlot>();

        public IReadOnlyList<GameDataSaveSlot> LoadedSlots => loaded_slots;

        public Fast.Callback<GameDataSaveSlot> OnLoadCurrentSlot => on_load_current_slot;
        public Fast.Callback<GameDataSaveSlot> OnSaveCurrentLost => on_save_current_slot;

        public async Task LoadAllSlots()
        {
            loaded_slots.Clear();
            current_slot = null;

            string base_folder = $"{Fast.FastService.MApplication.PersistentDataPath}Fast/GameSaveData/";

            Fast.FileUtils.CreateAllFilepathDirectories(base_folder);

            string[] directories = Directory.GetDirectories(base_folder);

            for (int i = 0; i < directories.Length; ++i)
            {
                string curr_directory = directories[i];

                string file_directory = $"{curr_directory}/data.save";

                GameDataSaveSlot data = await Fast.Serializers.JSONSerializer.DeSerializeFromPathAsync<GameDataSaveSlot>(file_directory);

                lock (loaded_slots)
                {
                    if (data == null)
                    {
                        continue;
                    }

                    bool already_loaded = SlotIsLoaded(data.Slot);

                    if(already_loaded)
                    {
                        continue;
                    }

                    loaded_slots.Add(data);
                }
            }
        }

        public async Task SaveSlot(int slot)
        {
            GameDataSaveSlot to_save = null;

            lock (loaded_slots)
            {
                for (int i = 0; i < loaded_slots.Count; ++i)
                {
                    GameDataSaveSlot curr_slot = loaded_slots[i];

                    if (curr_slot.Slot == slot)
                    {
                        to_save = curr_slot;

                        break;
                    }
                }

                if (to_save == null)
                {
                    return;
                }
            }

            string filepath = $"{Fast.FastService.MApplication.PersistentDataPath}Fast/GameSaveData/{slot}/data.save";

            bool success = await Fast.Serializers.JSONSerializer.SerializeToPathAsync(filepath, to_save);

            if(!success)
            {
                return;
            }
        }

        public async Task<bool> CreateSlot(int slot, string save_data_name)
        {
            if(slot < 0)
            {
                return false;
            }

            bool already_loaded = SlotIsLoaded(slot);

            if(already_loaded)
            {
                return false;
            }

            GameDataSaveSlot new_slot = new GameDataSaveSlot(slot, save_data_name);

            string filepath = $"{Fast.FastService.MApplication.PersistentDataPath}Fast/GameSaveData/{new_slot.Slot}/data.save";

            bool success = await Fast.Serializers.JSONSerializer.SerializeToPathAsync(filepath, new_slot);

            if (!success)
            {
                return false;
            }

            loaded_slots.Add(new_slot);

            return true;
        }

        public void RemoveSlot(int slot)
        {
            lock (loaded_slots)
            {
                bool found = false;

                for (int i = 0; i < loaded_slots.Count; ++i)
                {
                    if (loaded_slots[i].Slot == slot)
                    {
                        found = true;

                        loaded_slots.RemoveAt(i);

                        break;
                    }
                }

                if (!found)
                {
                    return;
                }
            }

            if(current_slot != null)
            {
                if(current_slot.Slot == slot)
                {
                    current_slot = null;
                }
            }

            string folderpath = $"{Fast.FastService.MApplication.PersistentDataPath}Fast/GameSaveData/{slot}/";

            Fast.FileUtils.DeleteFolderIfExists(folderpath);
        }

        private bool SlotIsLoaded(int slot)
        {
            lock (loaded_slots)
            {
                for (int i = 0; i < loaded_slots.Count; ++i)
                {
                    GameDataSaveSlot curr_slot = loaded_slots[i];

                    if (loaded_slots[i].Slot == slot)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool SetCurrentSlotData(int slot)
        {
            lock (loaded_slots)
            {
                for (int i = 0; i < loaded_slots.Count; ++i)
                {
                    GameDataSaveSlot curr_slot = loaded_slots[i];

                    if (loaded_slots[i].Slot == slot)
                    {
                        current_slot = curr_slot;

                        return true;
                    }
                }
            }

            return false;
        }

        public bool LoadCurrentSlotData()
        {
            if(current_slot == null)
            {
                return false;
            }

            on_load_current_slot.Invoke(current_slot);

            return true;
        }

        public bool SaveCurrentSlotData()
        {
            if (current_slot == null)
            {
                return false;
            }

            on_save_current_slot.Invoke(current_slot);

            return true;
        }
    }
}
