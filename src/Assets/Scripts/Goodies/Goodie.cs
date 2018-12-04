using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Goodie : MonoBehaviour
{
    public string goodieCode;
    public EventHandler OnPlayerConsumeGoodie;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            OnPlayerConsumeGoodie.Invoke();
        }
    }
}
