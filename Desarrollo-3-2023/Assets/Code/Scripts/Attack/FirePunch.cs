using UnityEngine;

namespace Code.Scripts.Attack
{
    public class FirePunch : MonoBehaviour
    {
        [SerializeField] private float speed = 20f;
        
        private Vector3 targetPos;
        
        public bool Move { get; private set; }
        
        private void Update()
        {
            if (!Move) return;
            
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed * Time.deltaTime);
            
            CheckPunchFinished();
        }

        private void CheckPunchFinished()
        {
            if (transform.localPosition == targetPos)
                Move = false;
        }

        public void Punch(float distance)
        {
            Transform trans = transform;
            
            Move = true;
            targetPos = trans.localPosition + trans.right * distance;
        }
    }
}
