using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.FOV
{
    /// <summary>
    /// Controls the field of view
    /// </summary>
    public class FieldOfView : MonoBehaviour
    {
        public float viewRadius;
        [Range(0, 360)]
        public float viewAngle;

        public LayerMask targetMask;
        public LayerMask obstacleMask;

        public List<Transform> visibleTargets = new List<Transform>();

        public bool searching = false;

        /// <summary>
        /// Toggles the finding of targets in the field of view.
        /// <remarks>
        /// If <paramref name="searching"/> is true, the field of view will start finding targets by calling <see cref="FindTargetsWithDelay"/>,
        /// otherwise it will stop finding targets by calling <see cref="StopCoroutine(string)"/> on the "FindTargetsWithDelay" coroutine.
        /// </remarks>
        /// <param name="searching">Whether to start or stop finding targets.</param>
        /// </summary>
        public void ToggleFindingTargets(bool searching = false)
        {
            this.searching = searching;


            if (searching)
                StartCoroutine(nameof(FindTargetsWithDelay), 0.1f);
            else
                StopCoroutine(nameof(FindTargetsWithDelay));
        }
        
        /// <summary>
        /// Returns a vector indicating the direction from the angle in degrees and whether the angle is global.
        /// </summary>
        /// <param name="angleInDegrees">The angle in degrees.</param>
        /// <param name="angleIsGlobal">Whether the angle is global.</param>
        /// <returns>A vector indicating the direction from the angle in degrees and whether the angle is global.</returns>
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.z;
            }

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0.0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        /// <summary>
        /// Finds all visible targets within the field of view at the specified interval.
        /// <remarks>
        /// This coroutine will run indefinitely until <see cref="searching"/> is set to false.
        /// </remarks>
        /// <param name="delay">The interval at which to find visible targets.</param>
        /// <returns>A coroutine that finds all visible targets within the field of view at the specified interval.</returns>
        /// </summary>
        IEnumerator FindTargetsWithDelay(float delay)
        {
            while (searching)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }
        
        /// <summary>
        /// Finds all visible targets within the field of view.
        /// <remarks>
        /// This method uses <see cref="Physics2D.OverlapCircleAll"/> to find all colliders within the view radius,
        /// then checks if the angle between the target and the field of view is within the view angle
        /// and if the target is not blocked by an obstacle.
        /// </remarks>
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
