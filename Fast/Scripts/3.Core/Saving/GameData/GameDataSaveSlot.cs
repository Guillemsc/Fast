using System;
using Newtonsoft.Json;

namespace Fast.Saving
{
    [System.Serializable]
    public class GameDataSaveSlot
    {
        [JsonProperty("slot")]
        private int slot = 0;

        [JsonProperty("name")]
        private string name = "";

        public GameDataSaveSlot(int slot, string name)
        {
            this.slot = slot;
            this.name = name;
        }

        [JsonIgnore] public int Slot => slot;
        [JsonIgnore] public string Name => name;
    }
}
