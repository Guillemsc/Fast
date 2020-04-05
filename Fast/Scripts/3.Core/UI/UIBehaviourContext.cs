using System;

namespace Fast.UI
{
    public class UIBehaviourContext
    {
        private Fast.UI.UIController controller = null;

        private Fast.PrefabScenes.PrefabScene<Fast.UI.Form> last_loaded_form = null;

        public UIBehaviourContext(Fast.UI.UIController controller)
        {
            this.controller = controller;
        }

        public Fast.UI.UIController Controller => controller;

        public Fast.PrefabScenes.PrefabScene<Fast.UI.Form> LastLoadedForm
        {
            get { return last_loaded_form; }
            set { last_loaded_form = value; }
        }
    }
}
