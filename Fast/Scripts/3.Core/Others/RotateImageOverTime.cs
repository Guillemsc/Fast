using System;
using UnityEngine;

namespace Fast.Others
{
    public class RotateImageOverTime : MonoBehaviour
    {
        [SerializeField] private float rotation_speed = 1.0f;

        void Update()
        {
            float rotation_speed_dt = UnityEngine.Time.deltaTime * rotation_speed;

            gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, rotation_speed_dt);
        }
    }

}
