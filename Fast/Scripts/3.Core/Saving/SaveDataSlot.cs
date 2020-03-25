using Newtonsoft.Json;
using System;

namespace Fast.Save
{
    [System.Serializable]
    public class SaveDataSlot
    {
        [JsonProperty("slot")]
        private int slot = 0;

        [JsonProperty("name")]
        private string name = "";

        public SaveDataSlot(int slot, string name)
        {
            this.slot = slot;
            this.name = name;
        }

        [JsonIgnore] public int Slot => slot;
        [JsonIgnore] public string Name => name;
    }
}
