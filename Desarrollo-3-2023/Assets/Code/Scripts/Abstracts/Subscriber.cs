using UnityEngine;
using kuznickiEventChannel;
using UnityEngine.Events;

public class Subscriber : MonoBehaviour
{
    [SerializeField] private EventChannelSO publisher;
    [SerializeField] private UnityEvent action;

    private void Start()
    {
        if (publisher != null)
            publisher.Subscribe(DoAction);
        else
            Debug.LogWarning("Warning: publisher not set in " + gameObject.name);
    }

    [ContextMenu("ActivateAction")]
    private void DoAction()
    {
        action.Invoke();
    }
}
