using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Fast.UIElements
{
    public class Toggle : MonoBehaviour
    {
        [SerializeField] private Button toggle_button = null;
        [SerializeField] private Image toggle_button_image = null;
        [SerializeField] private Image toggle_image = null;

        private bool is_on = false;

        private Fast.Callback<bool> on_value_change = new Callback<bool>();
        private Fast.Callback<bool> on_value_change_withouth_programatically = new Callback<bool>();

#if UNITY_EDITOR

        [MenuItem("GameObject/Fast/UI/Toggle", false, 10)]
        public static void CreateToggle()
        {
            GameObject toggle_go = new GameObject("Toggle");
            GameObject toggle_image_go = new GameObject("Toggle image");

            Toggle toggle = toggle_go.AddComponent<Toggle>();
            toggle.toggle_image = toggle_image_go.AddComponent<Image>();
            toggle.toggle_button_image = toggle_go.AddComponent<Image>();
            toggle.toggle_button = toggle_go.AddComponent<Button>();

            toggle.toggle_button_image.color = new UnityEngine.Color(0, 0, 0, 0);
            toggle.toggle_button.targetGraphic = toggle.toggle_button_image;

            toggle_image_go.transform.parent = toggle.transform;

            if (Selection.gameObjects.Length > 0)
            {
                toggle_go.transform.parent = Selection.gameObjects[0].transform;
            }
        }

#endif

        private void Awake()
        {
            InitButtons();

            SetIsOn(true);
        }

        private void InitButtons()
        {
            toggle_button.onClick.AddListener(OnToggleButtonDown);
        }

        private void OnToggleButtonDown()
        {
            bool changed = SetIsOn(!is_on);

            if(changed)
            {
                on_value_change_withouth_programatically.Invoke(is_on);
            }
        }

        private bool SetIsOn(bool set)
        {
            bool ret = set != is_on;

            is_on = set;

            if (toggle_image != null)
            {
                toggle_image.gameObject.SetActive(is_on);
            }

            if(ret)
            {
                on_value_change.Invoke(is_on);
            }

            return ret;
        }

        public bool IsOn
        {
            get { return is_on; }
        }

        public Callback<bool> OnValueChanged
        {
            get { return on_value_change; }
        }

        public Callback<bool> OnValueChangedWithoutProgramatically
        {
            get { return on_value_change_withouth_programatically; }
        }
    }
}
