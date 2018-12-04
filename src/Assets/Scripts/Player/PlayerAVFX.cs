using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAVFX : MonoBehaviour
{
    public InputState inputState;

    [Header("Booster")]
    public ParticleSystem boosterFlame;
    public AudioSource boosterSound;
    public float inputToSpeedFactor = 0.1f;
    public float maxIntensity;
    private float boosterFlameInitSpeed;

    [Header("Eyes")]
    public Transform eyeTransform;
    public Sprite eyeOpen;
    public Sprite eyeClose;
    public Sprite eyeSquint;
    public bool allowBlinking;
    public float blinkingIntervalMin;
    public float blinkingIntervalMax;
    public float blinkDuration;
    public float glanceRadius = 0.05f;
    public float glanceTrigger = 10f;
    private bool isSquinting = false;
    private SpriteRenderer eyes;
    private Vector2 eyeInitOffset;

    [Header("Flailing")]
    public float flailDuration;
    public float flailIntensity;
    public float relaxedIntensity;
    public FlailingLimb[] limbs;
    private bool allowFlailing;
    public AudioSource clinkSound;

    [Header("Particles")]
    public ParticleSystem catToIceParticle;
    public ParticleSystem hugRockParticle;
    public ParticleSystem breakRockParticle;

    [Header("Frozen")]
    public AudioSource freezeSound;
    public AudioSource breakIceSound;
    public AudioClip[] breakIceSounds;

    [Header("Getting Nearest Rock")]
    public float detectionRate = 0.3f;
    public float deltaDetectionRate = 0.3f;
    private Transform cachedNearestRock;

    private Player thisPlayer;

    void Start()
    {
        this.thisPlayer = GetComponentInParent<Player>();

        if (this.boosterFlame != null)
        {
            boosterFlameInitSpeed = boosterFlame.startSpeed;
        }
        eyeInitOffset = eyeTransform.localPosition;
        eyes = eyeTransform.GetComponent<SpriteRenderer>();
        StartCoroutine(BlinkEveryNowAndThen());
        for (var i = 0; i < limbs.Length; i++)
        {
            limbs[i].SetMovementIntensity(relaxedIntensity);
        }
    }

    void OnEnable()
    {
        StartCoroutine(BlinkEveryNowAndThen());
    }

    public void OnHitIce(Collision2D collision)
    {
        var position = collision.contacts[0].point;
        var normal = collision.contacts[0].normal;
        var rotation = Quaternion.LookRotation(Vector3.forward, normal);
        Instantiate(catToIceParticle, position, rotation);
    }

    public void OnHugRock(Collision2D collision)
    {
        Instantiate(
            hugRockParticle,
            transform.position,
            Quaternion.identity
        );
    }

    void Update()
    {
        if (!inputState)
        {
            return;
        }

        if (!inputState.IsEnabled)
        {
            return;
        }

        if (!this.thisPlayer)
        {
            this.thisPlayer = GetComponent<Player>();
        }

        if (boosterFlame != null)
        {
            if (inputState.HasInput && !this.thisPlayer.isFrozen)
            {
                if (!boosterFlame.isPlaying)
                {
                    boosterFlame.Play();
                    boosterSound.Play();
                }

                var intensityFactor = (inputState.JoyStick.value.magnitude * inputToSpeedFactor);
                intensityFactor = Mathf.Min(intensityFactor, maxIntensity);
                boosterFlame.startSpeed = boosterFlameInitSpeed + intensityFactor;
            }
            else
            {
                if (boosterFlame.isPlaying)
                {
                    boosterFlame.Stop();
                    boosterSound.Stop();
                }
            }
        }

        var nearestRock = GetNearestRock();
        if (nearestRock)
        {
            var vectorToRock = transform.InverseTransformVector(nearestRock.position - eyeTransform.position);
            var offset = Vector2.ClampMagnitude(vectorToRock, glanceRadius);
            eyeTransform.localPosition = Vector2.Lerp(eyeTransform.localPosition, eyeInitOffset + offset, 3f * Time.deltaTime);
        }

        if (isSquinting)
        {
            eyeTransform.localPosition = eyeInitOffset;
            eyes.sprite = eyeSquint;
        }
    }

    public void DontFlail()
    {
        allowFlailing = false;
        StartCoroutine(Relax());
    }

    public void AllowFlail()
    {
        allowFlailing = true;
    }

    public void Flail()
    {
        if (!allowFlailing)
        {
            return;
        }

        isSquinting = true;
        for (var i = 0; i < limbs.Length; i++)
        {
            limbs[i].SetMovementIntensity(flailIntensity);
        }

        this.clinkSound.Play();

        StartCoroutine(Relax());
    }

    IEnumerator Relax()
    {
        yield return new WaitForSeconds(flailDuration);
        for (var i = 0; i < limbs.Length; i++)
        {
            limbs[i].SetMovementIntensity(relaxedIntensity);
        }

        isSquinting = false;
    }

    IEnumerator BlinkEveryNowAndThen()
    {
        while(this.allowBlinking && this.isActiveAndEnabled)
        {
            if (eyes == null)
            {
                break;
            }

            if (!isSquinting)
            {
                eyes.sprite = eyeClose;
                yield return new WaitForSeconds(blinkDuration);
                eyes.sprite = eyeOpen;
            }

            var nextBlink = UnityEngine.Random.Range(blinkingIntervalMin, blinkingIntervalMax);
            yield return new WaitForSeconds(nextBlink);
        }
    }

    public void Freeze()
    {
        this.freezeSound.Play();
    }

    public void BreakIce()
    {
        var rng = Mathf.FloorToInt(UnityEngine.Random.Range(0, this.breakIceSounds.Length - 0.1f));
        this.breakIceSound.clip = this.breakIceSounds[rng];
        this.breakIceSound.Play();
    }
    
    private Transform GetNearestRock()
    {
        deltaDetectionRate -= Time.deltaTime;
        if (deltaDetectionRate > 0f)
        {
            return cachedNearestRock;
        }

        var rocks = new Collider2D[1];
        var rockCount = Physics2D.OverlapCircleNonAlloc(transform.position, glanceTrigger, rocks);
        if (rockCount <= 0)
        {
            return null;
        }

        var nearestRock = rocks[0];
        var distanceToNearestRock = Vector2.Distance(nearestRock.transform.position, rocks[0].transform.position);
        for (var i = 0; i < rocks.Length; i++)
        {
            if (rocks[i].transform.tag != "Rock")
            {
                continue;
            }

            var distance = Vector2.Distance(nearestRock.transform.position, rocks[0].transform.position);
            if (distance < distanceToNearestRock)
            {
                distanceToNearestRock = distance;
                nearestRock = rocks[i];
            }
        }

        cachedNearestRock = nearestRock.transform;
        return cachedNearestRock;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, glanceTrigger);
    //    var nearestRock = GetNearestRock();
    //    if (nearestRock)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(eyeTransform.position, nearestRock.position);
    //    }
    //}
}
