using UnityEngine;

namespace Code.Scripts.Attack
{
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

        public void Retrieve()
        {
            Move = true;
        }

        private void CheckRetrieveFinished()
        {
            if (transform.localPosition == startPos)
                Move = false;
        }
    }
}
