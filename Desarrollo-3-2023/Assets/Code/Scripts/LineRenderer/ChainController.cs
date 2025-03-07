using UnityEngine;

namespace Code.Scripts.LineRenderer
{
    /// <summary>
    /// Controls the chain
    /// </summary>
    public class ChainController : MonoBehaviour
    {
        [SerializeField] private Transform handTransform;
        [SerializeField] private Transform originTransform;

        private UnityEngine.LineRenderer lineRenderer;

        private void Start()
        {
            lineRenderer = GetComponent<UnityEngine.LineRenderer>();
        }

        private void Update()
        {
            lineRenderer.SetPosition(0, originTransform.position);
            lineRenderer.SetPosition(1, handTransform.position);
        }
    }
}