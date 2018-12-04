using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float triggeringCollisionSpeed;
    [Range(0f, 1f)] public float bounciness;
    public Ice playerIcePrefab;

    [Header("States")]
    public TransformState transformState;
    public GameState gameState;

    [Header("Events")]
    public Collision2DEvent OnHitIce;
    public Collision2DEvent OnHugRock;

    public bool enableRockHugging = true;

    [Header("Event Handlers")]
    public EventHandler OnHugRockEvt;
    public EventHandler OnAlienEatRock;
    public EventHandler OnHuggedRockBroke;
    public EventHandler OnCatFrozen;

    private Rock huggedRock;
    private CircleCollider2D coll;
    private PlayerAVFX avfx;
    [NonSerialized]
    public bool isFrozen = false;
    private PlayerMovement movement;
    private PlayerIceBreak iceBreak;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        iceBreak = GetComponent<PlayerIceBreak>();
        coll = GetComponent<CircleCollider2D>();
        avfx = GetComponentInChildren<PlayerAVFX>();
        OnHitIce = OnHitIce ?? new Collision2DEvent();
        OnHitIce.AddListener(avfx.OnHitIce);
        OnHugRock = OnHugRock ?? new Collision2DEvent();
        OnHugRock.AddListener(avfx.OnHugRock);

        OnAlienEatRock.OnInvoke.AddListener(() =>
        {
            if (huggedRock.state == RockState.Drifting)
            {
                huggedRock.Release();
                huggedRock = null;
                coll.enabled = true;
            }
        });

        OnCatFrozen.OnInvoke.AddListener(() =>
        {
            this.isFrozen = true;
            movement.enabled = false;
            iceBreak.enabled = true;
            var ice = Instantiate(playerIcePrefab);
            ice.transform.SetParent(transform);
            ice.transform.localPosition = Vector3.zero;
            coll.enabled = false;
            enableRockHugging = false;
            this.avfx.Freeze();
        });
    }

    void Update()
    {
        if (!transformState)
        {
            return;
        }

        if (!gameState)
        {
            return;
        }

        if (gameState.state == GameplayState.EndingCutscene)
        {
            Destroy(gameObject);
        }

        transformState.position = transform.position;
        if (huggedRock != null)
        {
            huggedRock.transform.position = transform.position;
            huggedRock.transform.rotation = transform.rotation;
            avfx.DontFlail();
        }
        else
        {
            avfx.AllowFlail();
            if (!coll.enabled)
            {
                coll.enabled = true;
            }

            if (gameState.state == GameplayState.HuggingRock)
            {
                gameState.state = GameplayState.Roaming;
            }
        }

        if (isFrozen)
        {
            var playerIce = transform.GetComponentInChildren<Ice>();
            if (!playerIce)
            {
                OnPlayerBreakIce();
            }
            else if(playerIce.hardness <= 0f)
            {
                OnPlayerBreakIce();
                Destroy(playerIce.gameObject);
            }
        }
    }

    void OnPlayerBreakIce()
    {
        movement.enabled = true;
        iceBreak.enabled = false;
        isFrozen = false;
        enableRockHugging = true;
    }
    
    public Rock GetHuggedRock()
    {
        return huggedRock;
    }

    public void OnContainingIceDestroyed()
    {
        var movement = GetComponent<PlayerMovement>();
        movement.enabled = true;
        var iceBreak = GetComponent<PlayerIceBreak>();
        iceBreak.enabled = false;
        this.isFrozen = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.transform.tag)
        {
            case "IceSprinkler":
                if (!this.isFrozen)
                {
                    OnCatFrozen.Invoke();
                }
                break;
            default:
                break;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        switch (collider.transform.tag)
        {
            case "IceSprinkler":
                if (!this.isFrozen)
                {
                    OnCatFrozen.Invoke();
                }
                break;
            default:
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case "Rock":
                if (huggedRock == null && enableRockHugging)
                {
                    var rock = collision.transform.GetComponent<Rock>();
                    if (rock.state == RockState.Drifting)
                    {
                        rock.GotHugged();
                        rock.breakingForce = this.triggeringCollisionSpeed;
                        huggedRock = rock;
                        coll.enabled = false;
                        OnHugRock.Invoke(collision);
                        OnHugRockEvt.Invoke();
                    }
                }

                break;
            case "Ice":
                OnHitIce.Invoke(collision);
                avfx.AllowFlail();
                avfx.Flail();
                OnHuggedRockBroke.Invoke();
                break;
            default:
                if (collision.relativeVelocity.magnitude >= this.triggeringCollisionSpeed)
                {
                    avfx.AllowFlail();
                    avfx.Flail();
                }

                break;
        }
    }
}
