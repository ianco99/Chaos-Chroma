using UnityEngine;

namespace Code.Scripts.Attack
{
    /// <summary>
    /// Component to control the throwing of the punch
    /// </summary>
    public class FirePunch : MonoBehaviour
    {
        [SerializeField] private float speed = 20f;
        
        private Vector3 targetPos;
        
        /// <summary>
        /// Is punch moving
        /// </summary>
        public bool Move { get; private set; }
        
        private void Update()
        {
            if (!Move) return;
            
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed * Time.deltaTime);
            
            CheckPunchFinished();
        }

        /// <summary>
        /// Stop moving if reached target position
        /// </summary>
        private void CheckPunchFinished()
        {
            if (transform.localPosition == targetPos)
                Move = false;
        }

        /// <summary>
        /// Initiate the punch
        /// </summary>
        /// <param name="distance">Distance to punch to</param>
        public void Punch(float distance)
        {
            Transform trans = transform;
            
            Move = true;
            targetPos = trans.localPosition + trans.right * distance;
        }
    }
}
