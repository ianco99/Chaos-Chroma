using UnityEngine;

namespace Code.Scripts.Door
{
    /// <summary>
    /// Closes the door
    /// </summary>
    public class DoorCloser : MonoBehaviour
    {
        public kuznickiEventChannel.VoidEventChannel closeDoorEventChannel;
        
        private readonly string playerTag = "Player";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(playerTag))
                CloseDoorEvent();
        }

        /// <summary>
        /// Raises the close door event channel
        /// </summary>
        private void CloseDoorEvent()
        {
            closeDoorEventChannel?.RaiseEvent();
        }
    }
}
