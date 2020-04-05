using System;

namespace Fast.Modules
{
    public class UIModule : UpdatableModule
    {
        private Fast.UI.UIController controller = new UI.UIController();

        public override void Start()
        {
            Time.TimeContext context = Fast.FastService.MTime.CreateTimeContext();
        }

        public override void Update()
        {
            controller.Update();
        }

        public void PlayBehaviour(Fast.UI.UIBehaviour behaviour)
        {
            controller.PlayBehaviour(behaviour);
        }
    }
}
