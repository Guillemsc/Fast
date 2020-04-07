using System;

namespace Fast.UI
{
    public class HideSubFormBehaviour : UIBehaviour
    {
        public HideSubFormBehaviour(Fast.PrefabScenes.PrefabSceneReference<Fast.UI.Form> form_ref, string animation)
        {
            Fast.UI.LoadOrGetFormInstruction ins1 = new Fast.UI.LoadOrGetFormInstruction(form_ref);
            Fast.UI.SetLastLoadedFormAsCurrentSubFormInstruction ins2 = new Fast.UI.SetLastLoadedFormAsCurrentSubFormInstruction();
            Fast.UI.CurrSubFormPlayAnimationInstruction ins3 = new Fast.UI.CurrSubFormPlayAnimationInstruction("FadeIn",
                Fast.UI.FormAnimationDirection.BACKWARD, false);
            Fast.UI.CurrSubFormSetActive ins4 = new Fast.UI.CurrSubFormSetActive(false);
            Fast.UI.UnloadCurrSubFormInstruction ins5 = new UnloadCurrSubFormInstruction();
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
