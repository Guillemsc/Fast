using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Modules
{
    public class LogicModule : UpdatableModule
    {
        private readonly Fast.Logic.GameLogicController controller = new Logic.GameLogicController(); 

        public override void Update()
        {
            controller.Update();
        }

        public async Task LoadAndSetCurrGameLogic(Fast.PrefabScenes.PrefabSceneReference<Fast.Logic.GameLogic> logic_reference)
        {
            await controller.LoadAndSetCurrGameLogic(logic_reference);
        }

        public async Task UnloadAndClearCurrGameLogic()
        {
            await controller.UnloadAndClearCurrGameLogic();
        }

        public void StartCurrGameLogic()
        {
            controller.StartCurrGameLogic();
        }

        public void FinishCurrGameLogic()
        {
            controller.FinishCurrGameLogic();
        }
    }
}
