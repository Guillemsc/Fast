using System;

namespace Fast.Modules
{
    public class UIModule : UpdatableModule
    {
        private Fast.UI.UIController controller = null;

        private Time.TimeContext time_context = null;

        public override void Start()
        {
            Time.TimeContext time_context = Fast.FastService.MTime.CreateTimeContext();

            controller = new UI.UIController(time_context);
        }

        public override void Update()
        {
            controller.Update();
        }

        public Time.TimeContext TimeContext => time_context;

        public void PlayBehaviour(Fast.UI.UIBehaviour behaviour)
        {
            controller.PlayBehaviour(behaviour);
        }
    }
}
