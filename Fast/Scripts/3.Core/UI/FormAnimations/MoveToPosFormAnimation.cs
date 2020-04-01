using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

namespace Fast.UI
{
    [Sirenix.OdinInspector.HideMonoScript]
    class MoveToPosFormAnimation : Fast.UI.FormAnimation
    {
        [System.Serializable]
        protected class MoveToPosData
        {
            [Sirenix.OdinInspector.LabelText("To move")]
            public List<GameObject> to_move = new List<GameObject>();

            [Sirenix.OdinInspector.LabelText("Start position")]
            public GameObject start_pos = null;

            [Sirenix.OdinInspector.LabelText("End position")]
            public GameObject end_pos = null;
        }

        [Sirenix.OdinInspector.Title("To move", "All the game objects that need to move, with the start and ending position")]
        [SerializeField] private List<MoveToPosData> data_to_move = new List<MoveToPosData>();
        [SerializeField] private Ease to_move_forward_ease = Ease.InOutQuad;
        [SerializeField] private Ease to_move_backwards_ease = Ease.InOutQuad;

        private Sequence sequence = null;

        private MoveToPosFormAnimation() : base("MoveToPos")
        {

        }

        protected override void TimeScaleChangedInternal(float time_scale)
        {
            if (sequence == null)
            {
                return;
            }

            sequence.timeScale = time_scale;
        }

        protected override void AnimateForwardInternal(float time_scale)
        {
            sequence = DOTween.Sequence();

            for (int i = 0; i < data_to_move.Count; ++i)
            {
                MoveToPosData curr_data = data_to_move[i];

                List<GameObject> to_move = curr_data.to_move;

                for (int y = 0; y < to_move.Count; ++y)
                {
                    GameObject curr_go = to_move[y];

                    curr_go.gameObject.SetActive(true);

                    Fast.Tweening.FadeTween fade_anim = new Fast.Tweening.FadeTween(curr_go, 0.0f, 1, 1, true);

                    Fast.Tweening.MoveTween move_anim
                        = new Fast.Tweening.MoveTween(curr_go, curr_data.start_pos.transform.localPosition,
                        curr_data.end_pos.transform.localPosition, 0.4f, ForceStartingValues);

                    if(ForceStartingValues)
                    {
                        fade_anim.SetStartingValuesForward();
                        move_anim.SetStartingValuesForward();
                    }

                    sequence.Join(fade_anim.AnimateForward());
                    sequence.Join(move_anim.AnimateForward());
                }
            }

            sequence.SetEase(to_move_forward_ease);
            sequence.timeScale = time_scale;

            sequence.OnComplete(Finish);

            sequence.Play();
        }

        protected override void AnimateBackwardInternal(float time_scale)
        {
            sequence = DOTween.Sequence();

            for (int i = 0; i < data_to_move.Count; ++i)
            {
                MoveToPosData curr_data = data_to_move[i];

                List<GameObject> to_move = curr_data.to_move;

                for (int y = 0; y < to_move.Count; ++y)
                {
                    GameObject curr_go = to_move[y];

                    curr_go.gameObject.SetActive(true);

                    Fast.Tweening.FadeTween fade_anim = new Tweening.FadeTween(curr_go, 0.0f, 1, 1, true);

                    Fast.Tweening.MoveTween move_anim
                        = new Fast.Tweening.MoveTween(curr_go, curr_data.start_pos.transform.position,
                        curr_data.end_pos.transform.position, 0.4f, ForceStartingValues);

                    if (ForceStartingValues)
                    {
                        fade_anim.SetStartingValuesBackward();
                        move_anim.SetStartingValuesBackward();
                    }

                    sequence.Join(fade_anim.AnimateForward());
                    sequence.Join(move_anim.AnimateForward());
                }
            }

            sequence.SetEase(to_move_backwards_ease);
            sequence.timeScale = time_scale;

            sequence.OnComplete(Finish);

            sequence.Play();
        }
    }
}