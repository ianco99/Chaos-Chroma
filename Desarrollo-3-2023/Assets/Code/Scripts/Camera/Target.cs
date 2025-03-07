using UnityEngine;

namespace Code.Scripts.Camera
{
    /// <summary>
    /// Controls the target
    /// </summary>
    public class Target : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;

        private void Update()
        {
            transform.Translate(transform.right * (speed * Time.deltaTime));
        }
    }
}
