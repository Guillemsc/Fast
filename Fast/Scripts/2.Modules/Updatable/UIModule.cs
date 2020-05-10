using System;

namespace Fast.Modules
{
    public class UIModule : UpdatableModule
    {
        private readonly Fast.UI.UIController controller = new Fast.UI.UIController();

        public void SetMainForm(Fast.UI.Form form)
        {
            controller.SetMainForm(form);
        }

        public void AddSubForm(Fast.UI.Form form)
        {
            controller.AddSubForm(form);
        }

        public void RemoveSubForm(Fast.UI.Form form)
        {
            controller.RemoveSubForm(form);
        }
    }
}
