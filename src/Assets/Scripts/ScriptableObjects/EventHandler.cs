using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(
    fileName = "EventHandler",
    menuName = "Cat's Ship/Event Handler",
    order = 0)]
public class EventHandler : ScriptableObject
{
    public UnityEvent OnInvoke = new UnityEvent();

    public void Invoke()
    {
        OnInvoke.Invoke();
    }
}
