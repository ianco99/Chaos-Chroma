using UnityEngine;

namespace Code.Scripts.Attack
{
    /// <summary>
    /// Component to control the retrieving of the punch
    /// </summary>
    public class RetrievePunch : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        
        private Vector3 startPos;
        
        public bool Move { get; private set; }

        private void Start()
        {
            startPos = transform.localPosition;
        }

        private void Update()
        {
            if (!Move) return;
            
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPos, speed * Time.deltaTime);
            
            CheckRetrieveFinished();
        }

        /// <summary>
        /// Initiate the retrieval
        /// </summary>
        public void Retrieve()
        {
            Move = true;
        }

        /// <summary>
        /// Stop retrieving if hand reached its position
        /// </summary>
        private void CheckRetrieveFinished()
        {
            if (transform.localPosition != startPos) return;
            
            Move = false;
            transform.rotation = Quaternion.identity;
        }
        
        /// <summary>
        /// Return if the hand is in its position
        /// </summary>
        /// <returns>true if it is</returns>
        public bool InPos()
        {
            return transform.localPosition == startPos;
        }
        
        /// <summary>
        /// Immediately set hand to original position and rotation
        /// </summary>
        public void Reset()
        {
            Move = false;
            
            Transform trans = transform;
            
            trans.localPosition = startPos;
            trans.rotation = Quaternion.identity;
        }
    }
}
