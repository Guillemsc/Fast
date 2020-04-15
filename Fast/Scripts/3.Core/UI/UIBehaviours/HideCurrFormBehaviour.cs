using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.UI
{
    public class HideCurrFormBehaviour : Fast.UI.UIBehaviour
    {
        public HideCurrFormBehaviour(string animation, bool unload = true)
        {
            Fast.UI.CurrFormPlayFormAnimationInstruction ins1 = new Fast.UI.CurrFormPlayFormAnimationInstruction("FadeIn",
                Fast.UI.FormAnimationDirection.BACKWARD, false);
            Fast.UI.CurrFormSetActive ins2 = new Fast.UI.CurrFormSetActive(false);
            Fast.UI.UnloadCurrFormInstruction ins3 = new UnloadCurrFormInstruction();
            Fast.UI.ClearCurrFormInstruction ins4 = new ClearCurrFormInstruction();

            AddInstruction(ins1);
            AddInstruction(ins2);

            if (unload)
            {
                AddInstruction(ins3);
            }

            AddInstruction(ins4);
        }
    }
}
