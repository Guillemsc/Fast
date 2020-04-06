using System;

namespace Fast.UI
{
    public class HideFormBehaviour : UIBehaviour
    {
        public HideFormBehaviour(string prefab_scene_name, string animation)
        {
            Fast.UI.LoadOrGetFormInstruction ins1 = new Fast.UI.LoadOrGetFormInstruction(prefab_scene_name);
            Fast.UI.SetLastLoadedFormAsCurrentFormInstruction ins2 = new Fast.UI.SetLastLoadedFormAsCurrentFormInstruction();
            Fast.UI.CurrFormPlayFormAnimInstruction ins3 = new Fast.UI.CurrFormPlayFormAnimInstruction("FadeIn",
                Fast.UI.FormAnimationDirection.BACKWARD, false);
            Fast.UI.CurrFormSetActive ins4 = new Fast.UI.CurrFormSetActive(false);
            Fast.UI.UnloadCurrFormInstruction ins5 = new UnloadCurrFormInstruction();
            Fast.UI.ClearCurrFormInstruction ins6 = new Fast.UI.ClearCurrFormInstruction();

            AddInstruction(ins1);
            AddInstruction(ins2);
            AddInstruction(ins3);
            AddInstruction(ins4);
            AddInstruction(ins5);
            AddInstruction(ins6);
        }
    }
}
