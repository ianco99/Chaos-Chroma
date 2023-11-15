using UnityEngine;

namespace Code.Scripts.Door
{
    public class DoorCloser : MonoBehaviour
    {
        public kuznickiEventChannel.VoidEventChannel closeDoorEventChannel;
        
        private readonly string playerTag = "Player";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(playerTag))
                CloseDoorEvent();
        }

        private void CloseDoorEvent()
        {
            closeDoorEventChannel?.RaiseEvent();
        }
    }
}
