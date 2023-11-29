using UnityEngine;

namespace Code.Scripts.Camera
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;

        private void Update()
        {
            transform.Translate(transform.right * (speed * Time.deltaTime));
        }
    }
}
