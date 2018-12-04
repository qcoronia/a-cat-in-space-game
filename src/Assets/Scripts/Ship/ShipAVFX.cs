using System;
using System.Collections;
using UnityEngine;

public class ShipAVFX : MonoBehaviour
{
    public TransformState transformState;
    public ColliderTriggerState openDoorTrigger;
    public ColliderTriggerState indoorTrigger;
    public ParticleSystem shipBooster;
    public AudioSource shipBoosterSound;
    public bool enableDoors = false;
    public bool isCatInside = false;

    [Header("Event Handlers")]
    public EventHandler OnCatEnteredShip;
    public EventHandler OnEndingCutsceneEnded;

    private Animator animator;
    private bool isEndingCutsceneQueued = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isOpened", false);
        transformState.position = transform.position;

        OnCatEnteredShip.OnInvoke.AddListener(QueueEndingCutscene);
    }

    void Update()
    {
        if (!enableDoors)
        {
            return;
        }

        isCatInside = false;
        if (openDoorTrigger.isTriggering)
        {
            animator.SetBool("isOpened", true);
        }

        if (!openDoorTrigger.isTriggering)
        {
            animator.SetBool("isOpened", false);
            if (indoorTrigger.isTriggering)
            {
                isCatInside = true;
                OnCatEnteredShip.Invoke();
            }
        }
    }

    public void EnableDoors()
    {
        enableDoors = true;
    }

    public void StartBoosters()
    {
        shipBooster.Play();
        shipBoosterSound.Play();
    }

    public void OnDoorClosed()
    {
        if (isEndingCutsceneQueued)
        {
            StartEndingCutscene();
        }
    }

    public void QueueEndingCutscene()
    {
        isEndingCutsceneQueued = true;
    }

    public void StartEndingCutscene()
    {
        animator.Play("EndingSequence");
    }

    public void EndEndingCutscene()
    {
        OnEndingCutsceneEnded.Invoke();
        shipBooster.Stop();
        shipBoosterSound.Stop();
    }

    /// <summary>
    /// Broadcasted from Ice Child
    /// </summary>
    public void OnContainingIceDestroyed()
    {
        EnableDoors();
    }
}
