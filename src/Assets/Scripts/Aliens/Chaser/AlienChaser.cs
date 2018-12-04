
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RoamMovement))]
[RequireComponent(typeof(ChaseMovement))]
[RequireComponent(typeof(CircleCollider2D))]
public class AlienChaser : MonoBehaviour
{
    public Ice alienIcePrefab;

    public GameState gameState;

    public TransformState target;
    public TransformState roamNear;

    private RoamMovement roamMovement;
    private ChaseMovement chaseMovement;
    public AlienChaserState state;
    public float breakFreeFromIceInterval;
    public AudioSource breakIceSound;
    public AudioClip[] breakIceSounds;

    public EventHandler OnAlienEatRock;

    private bool isFrozen;
    private Collider2D coll;

    void Start()
    {
        roamMovement = GetComponent<RoamMovement>();
        roamMovement.SetRoamNearTarget(roamNear);
        chaseMovement = GetComponent<ChaseMovement>();
        chaseMovement.SetTarget(target);
        this.coll = GetComponent<Collider2D>();

        state = AlienChaserState.Roaming;
        roamMovement.enabled = true;
        chaseMovement.enabled = false;
        isFrozen = false;
        OnAlienEatRock.OnInvoke.AddListener(AlienStateChange);
    }

    public void AlienStateChange()
    {
        if (this.isFrozen)
        {
            StartCoroutine(this.BreakFreeFromIce());
            return;
        }

        switch (gameState.state)
        {
            case GameplayState.HuggingRock:
                state = AlienChaserState.Chasing;
                roamMovement.enabled = false;
                chaseMovement.enabled = true;
                break;
            default:
                state = AlienChaserState.Roaming;
                roamMovement.enabled = true;
                chaseMovement.enabled = false;
                break;
        }
    }

    IEnumerator BreakFreeFromIce()
    {
        var iceComponent = GetComponentInChildren<Ice>();
        if (!iceComponent)
        {
            yield return new WaitWhile(() => false);
        }

        var maxIteration = 20;
        var currentIteration = 0;
        var initialBreakDone = false;
        while(this.isFrozen)
        {
            currentIteration++;
            if (currentIteration >= maxIteration)
            {
                break;
            }

            if (!initialBreakDone)
            {
                yield return new WaitForSeconds(this.breakFreeFromIceInterval);
            }
            
            if (!iceComponent)
            {
                iceComponent = GetComponentInChildren<Ice>();
            }

            if (iceComponent)
            {
                iceComponent.Damage();
                this.BreakIce();
                if (iceComponent.hardness <= 0)
                {
                    iceComponent.DestroyIce();
                }
            }
            else
            {
                this.isFrozen = false;
            }

            yield return new WaitForSeconds(this.breakFreeFromIceInterval);
        }
    }

    public void BreakIce()
    {
        var rng = Mathf.FloorToInt(UnityEngine.Random.Range(0, this.breakIceSounds.Length - 0.1f));
        this.breakIceSound.clip = this.breakIceSounds[rng];
        this.breakIceSound.Play();
    }

    public bool IsFrozen()
    {
        return this.isFrozen;
    }

    public void OnContainingIceDestroyed()
    {
        this.isFrozen = false;
        this.state = AlienChaserState.Roaming;
        this.roamMovement.enabled = true;
        this.chaseMovement.enabled = true;
        this.coll.enabled = true;
        this.AlienStateChange();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.transform.tag)
        {
            case "IceSprinkler":
                this.Freeze();
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
                this.Freeze();
                break;
            default:
                break;
        }
    }

    void Freeze()
    {
        var existingIce = GetComponentInChildren<Ice>();
        if (!!existingIce)
        {
            isFrozen = true;
            this.state = AlienChaserState.Frozen;
            this.roamMovement.enabled = false;
            this.chaseMovement.enabled = false;
            this.coll.enabled = false;
            this.AlienStateChange();
        }

        if (this.isFrozen)
        {
            return;
        }

        isFrozen = true;
        this.state = AlienChaserState.Frozen;
        this.roamMovement.enabled = false;
        this.chaseMovement.enabled = false;
        this.coll.enabled = false;
        this.AlienStateChange();
        var ice = Instantiate(alienIcePrefab);
        ice.transform.SetParent(transform);
        ice.transform.localPosition = alienIcePrefab.transform.position;
        ice.transform.localRotation = alienIcePrefab.transform.rotation;
        ice.transform.localScale = alienIcePrefab.transform.lossyScale;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            var player = other.transform.GetComponent<Player>();
            var rock = player.GetHuggedRock();
            if (rock != null)
            {
                if (rock && rock.state == RockState.Hugged)
                {
                    OnAlienEatRock.Invoke();
                }
            }
        }

        if (other.transform.tag == "Rock")
        {
            var rock = other.transform.GetComponent<Rock>();
            Instantiate(
                rock.breakRockParticle,
                Vector3.Lerp(transform.position, other.contacts[0].point, 0.5f),
                Quaternion.LookRotation(Vector3.forward, other.contacts[0].normal)
            );
            Instantiate(rock.breakRockSound, rock.transform.position, rock.transform.rotation);
            Destroy(rock.gameObject);
        }
    }
}

public enum AlienChaserState
{
    Roaming,
    Chasing,
    Frozen,
}
