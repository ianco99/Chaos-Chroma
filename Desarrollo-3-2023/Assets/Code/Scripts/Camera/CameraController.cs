using UnityEngine;

namespace Code.Scripts.Camera
{
    /// <summary>
    /// Controls the camera
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = Vector3.zero;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float minHeight = -4f;
        
        private void FixedUpdate()
        {
            if (!target) return;
            Vector3 vec = target.position + offset;
            vec.y = Mathf.Max(vec.y, minHeight);
            
            transform.position = Vector3.Lerp(transform.position, vec, speed * Time.deltaTime);
        }
    }
}