using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fast.UI
{
    public class ButtonExtension : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float long_press_time_seconds = 1.0f;

        private Button button = null;

        private Fast.Time.TimerStopwatch timer = new Time.TimerStopwatch();

        private bool down = false;

        private bool can_long_press = false;

        private Fast.Callback<ButtonExtension> on_down = new Callback<ButtonExtension>();
        private Fast.Callback<ButtonExtension> on_up = new Callback<ButtonExtension>();
        private Fast.Callback<ButtonExtension> on_enter = new Callback<ButtonExtension>();
        private Fast.Callback<ButtonExtension> on_exit = new Callback<ButtonExtension>();
        private Fast.Callback<ButtonExtension> on_long_press = new Callback<ButtonExtension>();

        public Button Button => button;

        public Fast.Callback<ButtonExtension> OnDown => on_down;
        public Fast.Callback<ButtonExtension> OnUp => on_up;
        public Fast.Callback<ButtonExtension> OnEnter => on_enter;
        public Fast.Callback<ButtonExtension> OnExit => on_exit;
        public Fast.Callback<ButtonExtension> OnLongPress => on_long_press;

        private void Awake()
        {
            TryGetButton();
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                UpdateLongPress();
                UpdateDoubleClick();
            }
        }

        private void TryGetButton()
        {
            button = gameObject.GetComponent<Button>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            on_down.Invoke(this);

            down = true;

            can_long_press = true;

            timer.Start();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            on_up.Invoke(this);

            down = false;

            timer.Reset();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            on_enter.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            on_exit.Invoke(this);
        }

        private void UpdateLongPress()
        {
            if(!can_long_press)
            {
                return;
            }

            if (down)
            {
                if (timer.ReadTime().TotalSeconds >= long_press_time_seconds)
                {
                    on_long_press.Invoke(this);

                    can_long_press = false;
                }
            }
        }

        private void UpdateDoubleClick()
        {

        }
    }
}
