using kuznickiEventChannel;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.Abstracts
{
    /// <summary>
    /// Base class for subscribers
    /// </summary>
    public class Subscriber : MonoBehaviour
    {
        [SerializeField] private EventChannelSO publisher;
        [SerializeField] private UnityEvent action;

        /// <summary>
        /// Subscribes to the publisher's channel in the start method,
        /// warning the user if the publisher is not set.
        /// </summary>
        private void Start()
        {
            if (publisher != null)
                publisher.Subscribe(DoAction);
            else
                Debug.LogWarning("Warning: publisher not set in " + gameObject.name);
        }

        /// <summary>
        /// Invokes the UnityEvent action when called.
        /// </summary>
        [ContextMenu("ActivateAction")]
        private void DoAction()
        {
            action.Invoke();
        }
    }
}