using UnityEngine;

namespace Code.Scripts.Enemy
{
    /// <summary>
    /// Player detector manager
    /// </summary>
    public class BossAreaManager : MonoBehaviour
    {
        private GameObject playerInArea;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            playerInArea = other.gameObject;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject != playerInArea)
                return;
            
            playerInArea = null;
        }

        /// <summary>
        /// Get if player is in area
        /// </summary>
        /// <returns>true if it is, false if it isn't</returns>
        public bool IsPlayerInArea()
        {
            return playerInArea != null;
        }

        /// <summary>
        /// Gives the difference between the detected player and the center of the area
        /// </summary>
        /// <returns>difference in x axis</returns>
        public float GetPositionDifference()
        {
            if (playerInArea == null)
                return 0;

            return playerInArea.transform.position.x - transform.position.x;
        }
    }
}