using UnityEngine;

namespace Code.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = Vector3.zero;
        [SerializeField] private float speed = 10f;

        private void LateUpdate()
        {
            if (!target) return;
            
            transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
        }
    }
}