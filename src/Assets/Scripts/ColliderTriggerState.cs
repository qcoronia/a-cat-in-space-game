using System;
using UnityEngine;

public class ColliderTriggerState : MonoBehaviour
{

    public bool isTriggerStarted = false;
    public bool isTriggering = false;
    public bool isTriggerEnded = false;

    public Collider2D triggerInfo;

    public string[] detectableTags;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsDetectable(other.transform.tag))
        {
            return;
        }

        isTriggerStarted = true;
        isTriggering = false;
        isTriggerEnded = false;
        triggerInfo = other;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!IsDetectable(other.transform.tag))
        {
            return;
        }

        isTriggerStarted = false;
        isTriggering = true;
        isTriggerEnded = false;
        triggerInfo = other;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!IsDetectable(other.transform.tag))
        {
            return;
        }

        isTriggerStarted = false;
        isTriggering = false;
        isTriggerEnded = true;
        triggerInfo = null;
    }

    public bool IsDetectable(string tag)
    {
        for (var i = 0; i < detectableTags.Length; i++)
        {
            if (tag == detectableTags[i])
            {
                return true;
            }
        }

        return false;
    }
}
