using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Fast.UIHelpers
{
    public class ButtonClickEx : MonoBehaviour
    {
        [SerializeField] private float double_click_time_sec = 0.15f;

        private bool inited = false;

        private Button button = null;

        private Timer double_click_timer = new Timer();
        private float first_click_time = 0.0f;
        private bool wating_for_first_click = true;

        private Fast.Callback<Button> on_button_click = new Callback<Button>();
        private Fast.Callback<Button> on_button_single_click = new Callback<Button>();
        private Fast.Callback<Button> on_button_double_click = new Callback<Button>();

        private void Start()
        {
            if (button == null)
            {
                button = gameObject.GetComponent<Button>();
            }

            InitButton();
        }

        private void Update()
        {
            CheckSingleClick();
        }

        private void FixedUpdate()
        {
            CheckSingleClick();
        }

        private void LateUpdate()
        {
            CheckSingleClick();
        }

        private void InitButton()
        {
            if (!inited)
            {
                if (button != null)
                {
                    button.onClick.AddListener(OnButtonDown);

                    double_click_timer.Start();

                    wating_for_first_click = true;

                    inited = true;
                }
            }
        }

        public void SetButton(Button button)
        {
            this.button = button;

            InitButton();
        }

        private void OnButtonDown()
        {
            on_button_click.Invoke(button);

            CheckDoubleClick();
        }

        private void CheckSingleClick()
        {
            if (!wating_for_first_click)
            {
                if (double_click_timer.ReadUnscaledTime() - first_click_time >= double_click_time_sec)
                {
                    wating_for_first_click = true;

                    on_button_single_click.Invoke(button);
                }
            }
        }

        private void CheckDoubleClick()
        {
            if (wating_for_first_click)
            {
                first_click_time = double_click_timer.ReadUnscaledTime();

                wating_for_first_click = false;
            }
            else
            {
                on_button_double_click.Invoke(button);

                wating_for_first_click = true;
            }
        }

        public float DoubleClickTimeSec
        {
            get { return double_click_time_sec; }
            set { double_click_time_sec = value; }
        }

        public Fast.Callback<Button> OnButtonClick
        {
            get { return on_button_click; }
        }

        public Fast.Callback<Button> OnButtonSingleClick
        {
            get { return on_button_single_click; }
        }

        public Fast.Callback<Button> OnButtonDoubleClick
        {
            get { return on_button_double_click; }
        }
    }
}
