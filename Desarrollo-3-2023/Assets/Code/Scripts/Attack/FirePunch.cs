using UnityEngine;

namespace Code.Scripts.Attack
{
    /// <summary>
    /// Component to control the throwing of the punch
    /// </summary>
    public class FirePunch : MonoBehaviour
    {
        [SerializeField] private float speed = 20f;
        [SerializeField] private bool isLeft;
                
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
        /// <param name="pos">Position to punch to</param>
        public void Punch(Vector2 pos)
        {
            Move = true;
            targetPos = pos;

            float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(isLeft ? angle : angle + 180, Vector3.forward);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Stop();
        }

        /// <summary>
        /// Stop moving punches and reset the object
        /// </summary>
        public void Stop()
        {
            transform.rotation = Quaternion.identity;
            Move = false;
        }
    }
}
