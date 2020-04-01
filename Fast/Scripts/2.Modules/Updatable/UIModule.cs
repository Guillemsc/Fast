using System;

namespace Fast.Modules
{
    public class UIModule : UpdatableModule
    {
        private Fast.UI.UIController controller = null;

        public override void Start()
        {
            Time.TimeContext context = Fast.FastService.MTime.CreateTimeContext();

            controller = new UI.UIController(context);
        }

        public override void Update()
        {
            controller.Update();
        }

        public Fast.UI.UIBehaviour CreateBehaviour(int instruction_index)
        {
            return controller.CreateBehaviour(instruction_index);
        }

        public void PlayBehaviour(int instruction_index)
        {
            controller.PlayBehaviour(instruction_index);
        }
    }
}
