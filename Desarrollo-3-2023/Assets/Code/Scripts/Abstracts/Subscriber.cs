using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kuznickiEventChannel;
using UnityEngine.UIElements;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;

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

    [ContextMenu("MeOdio")]
    private void DoAction()
    {
        action.Invoke();
    }
}
