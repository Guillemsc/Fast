using System;

namespace Fast.UI
{
    public class ShowFormBehaviour : UIBehaviour
    {
        public ShowFormBehaviour(string prefab_scene_name, string animation)
        {
            Fast.UI.LoadOrGetFormInstruction ins1 = new Fast.UI.LoadOrGetFormInstruction(prefab_scene_name);
            Fast.UI.SetLastLoadedFormAsCurrentFormInstruction ins2 = new Fast.UI.SetLastLoadedFormAsCurrentFormInstruction();
            Fast.UI.SetCurrFormAnimationStartingValues ins3 = new Fast.UI.SetCurrFormAnimationStartingValues("FadeIn", 
                FormAnimationDirection.FORWARD);
            Fast.UI.CurrFormSetActive ins4 = new Fast.UI.CurrFormSetActive(true);
            Fast.UI.CurrFormPlayFormAnimInstruction ins5 = new Fast.UI.CurrFormPlayFormAnimInstruction("FadeIn", 
                Fast.UI.FormAnimationDirection.FORWARD, true);

            AddInstruction(ins1);
            AddInstruction(ins2);
            AddInstruction(ins3);
            AddInstruction(ins4);
            AddInstruction(ins5);
        }
    }
}
