using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Modules
{
    public class SaveDataModule : Module
    {
        private readonly Fast.Save.SaveController controller = new Save.SaveController();

        public Fast.Callback<Fast.Save.SaveDataSlot> OnLoadCurrentSlot => controller.OnLoadCurrentSlot;
        public Fast.Callback<Fast.Save.SaveDataSlot> OnSaveCurrentSlot => controller.OnSaveCurrentLost;

        public IReadOnlyList<Fast.Save.SaveDataSlot> LoadedSlots => controller.LoadedSlots;

        public async Task LoadAllSlots()
        {
            await controller.LoadAllSlots();
        }

        public async Task SaveSlot(int slot)
        {
            await controller.SaveSlot(slot);
        }

        public async Task<bool> CreateSlot(int slot, string save_data_name)
        {
            return await controller.CreateSlot(slot, save_data_name);
        }

        public void SetCurrentSlotData(int slot)
        {
            controller.SetCurrentSlotData(slot);
        }

        public void LoadCurrentSlotData()
        {
            controller.LoadCurrentSlotData();
        }

        public void SaveCurrentSlotDataData()
        {
            controller.SaveCurrentSlotData();
        }
    }
}
