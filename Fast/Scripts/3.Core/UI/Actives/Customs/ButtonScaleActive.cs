using System;
using UnityEngine;
using DG.Tweening;

namespace Fast.UI.Actives
{
    public class ButtonScaleActive : DOTweenActive
    {
        [SerializeField] private ButtonExtension button = null;

        [SerializeField] private Vector2 up_scale = Vector2.one;
        [SerializeField] private Vector2 down_scale = Vector2.one;
        [SerializeField] private Vector2 out_scale = Vector2.one;
        [SerializeField] private Vector2 in_scale = Vector2.one;

        private void Awake()
        {
            InitButton();

            ScaleDown();
        }

        private void InitButton()
        {
            if(button == null)
            {
                return;
            }

            button.OnDown.Subscribe(OnButtonDown);
            button.OnUp.Subscribe(OnButtonUp);
            button.OnEnter.Subscribe(OnButtonIn);
            button.OnExit.Subscribe(OnButtonOut);
        }

        private void ScaleDown()
        {
            if(button == null)
            {
                return;
            }

            if (seq != null)
            {
                seq.Kill();
            }

            Tween tween = button.gameObject.transform.DOScale(down_scale, Duration);

            SetEasing(tween);

            seq.Append(tween);

            seq.Play();
        }

        private void ScaleUp()
        {
            if (button == null)
            {
                return;
            }

            if (seq != null)
            {
                seq.Kill();
            }

            Tween tween = button.gameObject.transform.DOScale(up_scale, Duration);

            SetEasing(tween);

            seq.Append(tween);

            seq.Play();
        }

        private void ScaleIn()
        {
            if (button == null)
            {
                return;
            }

            if (seq != null)
            {
                seq.Kill();
            }

            Tween tween = button.gameObject.transform.DOScale(in_scale, Duration);

            SetEasing(tween);

            seq.Append(tween);

            seq.Play();
        }

        private void ScaleOut()
        {
            if (button == null)
            {
                return;
            }

            if (seq != null)
            {
                seq.Kill();
            }

            Tween tween = button.gameObject.transform.DOScale(out_scale, Duration);

            SetEasing(tween);

            seq.Append(tween);

            seq.Play();
        }

        private void OnButtonDown(ButtonExtension button)
        {
            ScaleDown();
        }

        private void OnButtonUp(ButtonExtension button)
        {
            ScaleUp();
        }

        private void OnButtonIn(ButtonExtension button)
        {
            ScaleIn();
        }

        private void OnButtonOut(ButtonExtension button)
        {
            ScaleOut();
        }
    }
}
