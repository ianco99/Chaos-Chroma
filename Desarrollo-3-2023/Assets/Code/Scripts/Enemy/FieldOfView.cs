using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.FieldOfView
{
    public class FieldOfView : MonoBehaviour
    {
        public float viewRadius;
        [Range(0, 360)]
        public float viewAngle;
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if(!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0.0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}
