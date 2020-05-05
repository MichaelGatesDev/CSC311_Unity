using System;
using UnityEngine;

namespace UI
{
    public class AlwaysFaceCamera : MonoBehaviour
    {
        public Camera Target;

        private void Update()
        {
            var lookDir = transform.position - Target.transform.position * 2;
            lookDir.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }
}