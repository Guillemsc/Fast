using System;

namespace Fast.UI
{
    public class ShowSubFormBehaviour : UIBehaviour
    {
        public ShowSubFormBehaviour(Fast.PrefabScenes.PrefabSceneReference<Fast.UI.Form> form_ref, string animation)
        {
            Fast.UI.LoadOrGetFormInstruction ins1 = new Fast.UI.LoadOrGetFormInstruction(form_ref);
            Fast.UI.SetLastLoadedFormAsCurrentSubFormInstruction ins2 = new Fast.UI.SetLastLoadedFormAsCurrentSubFormInstruction();
            Fast.UI.SetCurrSubFormAnimationStartingValues ins3 = new Fast.UI.SetCurrSubFormAnimationStartingValues("FadeIn",
                FormAnimationDirection.FORWARD);
            Fast.UI.CurrSubFormSetActive ins4 = new Fast.UI.CurrSubFormSetActive(true);
            Fast.UI.CurrSubFormPlayAnimationInstruction ins5 = new Fast.UI.CurrSubFormPlayAnimationInstruction("FadeIn",
                Fast.UI.FormAnimationDirection.FORWARD, true);

            AddInstruction(ins1);
            AddInstruction(ins2);
            AddInstruction(ins3);
            AddInstruction(ins4);
            AddInstruction(ins5);
        }
    }
}
