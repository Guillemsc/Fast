using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Fast.Localization
{
    public enum LocalizationTextDisplayType //SimonCasual
    {
        AS_IT_IS,
        ALL_TO_UPPER,
        FIRST_CHAR_TO_UPPER,
        ALL_TO_LOWER,
    }

    [Sirenix.OdinInspector.HideMonoScript]
    [RequireComponent(typeof(TMPro.TextMeshProUGUI))]
    class LocalizationText : MonoBehaviour
    {
        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.Title("Key", "The key value to look for the localized text")]
        [Sirenix.OdinInspector.Required]
        [SerializeField]
        private string key = "";

        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.Title("Display", "The way in which the text will be displayed")]
        [Sirenix.OdinInspector.EnumToggleButtons]
        [SerializeField]
        private LocalizationTextDisplayType display_type = new LocalizationTextDisplayType();

        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.Title("Text before", "Text to show before the localized text")]
        [SerializeField]
        private string text_before = "";

        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.Title("Text after", "Text to show after the localized text")]
        [SerializeField]
        private string text_after = "";

        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.Title("Arguments", "Argument variables to replace in the localized text")]
        [SerializeField]
        private List<string> arguments = new List<string>();

        private TMPro.TextMeshProUGUI text = null;

        [SerializeField]
        private string simon_casual;

        private void Awake()
        {
            text = gameObject.GetComponent<TMPro.TextMeshProUGUI>();

            this.transform.SetAsFirstSibling();
        }

        private void Start()
        {
            FastInstance.Instance.MLocalization.AddLocalizationText(this);

            UpdateText();
        }

        public void UpdateText()
        {
            if (text != null)
            {
                string localized_text = FastInstance.Instance.MLocalization.GetLocalizedText(key, arguments);

                string text_to_set = text_before + localized_text + text_after;

                switch(display_type)
                {
                    case LocalizationTextDisplayType.ALL_TO_LOWER:
                        {
                            text_to_set = text_to_set.ToLower();
                            break;
                        }

                    case LocalizationTextDisplayType.ALL_TO_UPPER:
                        {
                            text_to_set = text_to_set.ToUpper();
                            break;
                        }

                    case LocalizationTextDisplayType.FIRST_CHAR_TO_UPPER:
                        {
                            text_to_set = text_to_set.FirstCharToUpper();
                            break;
                        }
                }

                text.text = text_to_set;
            }
        }
    }
}
