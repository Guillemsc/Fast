using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.UI
{
    public class HideAllSubFormsBehaviour : UIBehaviour
    {
        public HideAllSubFormsBehaviour(bool unload = true)
        {
            AllSubFormsPlayFormDefaultAnimationInstruction ins1 = new AllSubFormsPlayFormDefaultAnimationInstruction(
                FormAnimationDirection.BACKWARD, false);
            AllSubFormsSetActive ins2 = new AllSubFormsSetActive(false);
            UnloadAllSubFormsInstruction ins3 = new UnloadAllSubFormsInstruction();
            ClearAllSubForms ins4 = new ClearAllSubForms();

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
