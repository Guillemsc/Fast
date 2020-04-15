using System;

namespace Fast.UI
{
    public class ShowFormBehaviour : UIBehaviour
    {
        public ShowFormBehaviour(Fast.PrefabScenes.PrefabSceneReference<Fast.UI.Form> form_ref, string animation)
        {
            Fast.UI.LoadOrGetFormInstruction ins1 = new Fast.UI.LoadOrGetFormInstruction(form_ref);
            Fast.UI.SetLastLoadedFormAsCurrentFormInstruction ins2 = new Fast.UI.SetLastLoadedFormAsCurrentFormInstruction();
            Fast.UI.SetCurrFormAnimationStartingValues ins3 = new Fast.UI.SetCurrFormAnimationStartingValues("FadeIn", 
                FormAnimationDirection.FORWARD);
            Fast.UI.CurrFormSetActive ins4 = new Fast.UI.CurrFormSetActive(true);
            Fast.UI.CurrFormPlayFormAnimationInstruction ins5 = new Fast.UI.CurrFormPlayFormAnimationInstruction("FadeIn", 
                Fast.UI.FormAnimationDirection.FORWARD, true);

            AddInstruction(ins1);
            AddInstruction(ins2);
            AddInstruction(ins3);
            AddInstruction(ins4);
            AddInstruction(ins5);
        }
    }
}
