using System;
using System.Threading.Tasks;
using Fast.PrefabScenes;

namespace Fast.Logic
{
    public class GameLogicController : Fast.IController, Fast.IUpdatable
    {
        private PrefabScene<Fast.Logic.GameLogic> curr_logic = null;

        public void Update()
        {
            UpdateCurrLogic();
        }

        public async Task LoadAndSetCurrGameLogic(Fast.PrefabScenes.PrefabSceneReference<Fast.Logic.GameLogic> logic_reference)
        {
            if (logic_reference == null)
            {
                return;
            }

            if(curr_logic != null)
            {
                return;
            }

            PrefabScene<Fast.Logic.GameLogic> prefab_scene = 
                await Fast.FastService.MPrefabScenes.LoadPrefabSceneAsync(logic_reference);

            if(prefab_scene == null)
            {
                return;
            }

            curr_logic = prefab_scene;
        }

        public async Task UnloadAndClearCurrGameLogic()
        {
            if (curr_logic == null)
            {
                return;
            }

            await Fast.FastService.MPrefabScenes.UnloadPrefabSceneAsync(curr_logic);

            curr_logic = null;
        }

        public void StartCurrGameLogic()
        {
            if (curr_logic == null)
            {
                return;
            }

            curr_logic.Instance.StartLogic();
        }

        public void FinishCurrGameLogic()
        {
            if (curr_logic == null)
            {
                return;
            }

            curr_logic.Instance.FinishLogic();
        }

        private void UpdateCurrLogic()
        {
            if (curr_logic == null)
            {
                return;
            }

            curr_logic.Instance.UpdateLogic();
        }
    }
}
