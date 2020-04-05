using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Fast.UI
{
    public class UnloadCurrFormInstruction : Fast.UI.UIInstruction
    {
        public UnloadCurrFormInstruction()
        {
            
        }

        protected override void StartInternal(Fast.UI.UIBehaviourContext context)
        {
            Fast.FastService.MPrefabScenes.UnloadPrefabScene(context.Controller.CurrForm);
        }
    }
}
