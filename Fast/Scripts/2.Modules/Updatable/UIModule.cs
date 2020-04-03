using System;

namespace Fast.Modules
{
    public class UIModule : UpdatableModule
    {

        public override void Start()
        {
            Time.TimeContext context = Fast.FastService.MTime.CreateTimeContext();
        }

    }
}
