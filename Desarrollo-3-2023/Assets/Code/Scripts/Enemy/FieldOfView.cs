using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.FOV
{
    public class FieldOfView : MonoBehaviour
    {
        public float viewRadius;
        [Range(0, 360)]
        public float viewAngle;

        public LayerMask targetMask;
        public LayerMask obstacleMask;

        public List<Transform> visibleTargets = new List<Transform>();

        public bool searching = false;

        public void ToggleFindingTargets(bool searching = false)
        {
            this.searching = searching;


            if (searching)
                StartCoroutine(nameof(FindTargetsWithDelay), 0.1f);
            else
                StopCoroutine(nameof(FindTargetsWithDelay));
        }
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.z;
            }

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0.0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        IEnumerator FindTargetsWithDelay(float delay)
        {
            while (searching)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }
        private void FindVisibleTargets()
        {
            visibleTargets.Clear();
            Collider2D[] targetsInViewRadius;

            targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.right, dirToTarget) < viewAngle / 2.0f)
                {
                    float distToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics2D.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                    {
                        visibleTargets.Add(target);
                    }
                }
            }
        }
    }
}
