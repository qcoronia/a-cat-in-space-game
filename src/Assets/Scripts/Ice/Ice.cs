using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Ice : MonoBehaviour
{
    public int hardness = 1;
    public ParticleSystem iceShardParticle;
    public HardnessDetail[] hardnessDetails;

    [Header("Audio")]
    public AudioSource iceBrokenSound;

    [Header("Events")]
    public EventHandler onHuggerRockHitIce;
    public EventHandler onIceDestroyed;

    private SpriteRenderer spriteRenderer;
    private int initialHardness;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialHardness = hardness;
    }

    public void Damage()
    {
        if (this == null)
        {
            return;
        }
        
        hardness -= 1;

        var hardnessDetail = hardnessDetails
            .Where(e => e.hardnessLevel * initialHardness >= hardness)
            .OrderBy(e => e.hardnessLevel)
            .FirstOrDefault();
        if (hardnessDetail != null)
        {
            if (!spriteRenderer)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            spriteRenderer.sprite = hardnessDetail.sprite;
        }
        
        Instantiate(
            iceShardParticle,
            transform.position,
            Quaternion.AngleAxis(
                UnityEngine.Random.Range(0f, 360f),
                Vector3.forward
            )
        );
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag != "Rock")
        {
            return;
        }

        var rock = other.transform.GetComponent<Rock>();
        if (rock == null)
        {
            return;
        }

        if (rock.state != RockState.Hugged)
        {
            return;
        }


        this.Damage();

        if (onHuggerRockHitIce)
        {
            onHuggerRockHitIce.Invoke();
        }

        if (hardness <= 0)
        {
            this.DestroyIce();
        }
    }

    public void DestroyIce()
    {
        if (onIceDestroyed)
        {
            onIceDestroyed.Invoke();
        }

        var sound = Instantiate(this.iceBrokenSound, transform.position, transform.rotation);
        Destroy(sound, this.iceBrokenSound.clip.length + 0.5f);
        spriteRenderer.sprite = null;
        Destroy(gameObject, 0.2f);
        SendMessageUpwards("OnContainingIceDestroyed", SendMessageOptions.DontRequireReceiver);
    }
}

[Serializable]
public class HardnessDetail
{
    [Range(0f, 1f)]
    public float hardnessLevel;
    public Sprite sprite;
}
