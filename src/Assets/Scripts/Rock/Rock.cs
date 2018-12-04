
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rock : MonoBehaviour
{
    public float velocityMin;
    public float velocityMax;
    public float spinMin;
    public float spinMax;
    public int density;
    public float breakingForce;
    public RockState state;
    public ParticleSystem breakRockParticle;
    public bool breakOnRockContact;
    public AudioSource breakRockSound;
    public AudioSource hugRockSound;

    private Rigidbody2D rb;

    void Start()
    {
        var randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        var randomVelocity = Random.Range(velocityMin, velocityMax);

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(randomDir * randomVelocity, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(spinMin, spinMax));

        state = RockState.Drifting;
    }

    void Update()
    { }

    public void GotHugged()
    {
        state = RockState.Hugged;
        var sound = Instantiate(this.hugRockSound, transform.position, transform.rotation);
        Destroy(sound, this.hugRockSound.clip.length + 0.5f);
    }

    public void Release()
    {
        state = RockState.Drifting;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case "Rock":
                if (state == RockState.Hugged)
                {
                    Instantiate(
                        breakRockParticle,
                        Vector3.Lerp(transform.position, collision.contacts[0].point, 0.5f),
                        Quaternion.LookRotation(Vector3.forward, collision.contacts[0].normal)
                    );
                    Instantiate(breakRockSound, transform.position, transform.rotation);
                    Destroy(gameObject);
                }

                if (state == RockState.Drifting && breakOnRockContact)
                {
                    if (density <= 1)
                    {
                        Instantiate(
                            breakRockParticle,
                            Vector3.Lerp(transform.position, collision.contacts[0].point, 0.5f),
                            Quaternion.LookRotation(Vector3.forward, collision.contacts[0].normal)
                        );
                        Instantiate(breakRockSound, transform.position, transform.rotation);
                        Destroy(gameObject);
                        break;
                    }

                    density -= 1;
                }

                break;
            case "Ice":
                if (state == RockState.Hugged)
                {
                    Instantiate(
                        breakRockParticle,
                        Vector3.Lerp(transform.position, collision.contacts[0].point, 0.5f),
                        Quaternion.LookRotation(Vector3.forward, collision.contacts[0].normal)
                    );
                    Instantiate(breakRockSound, transform.position, transform.rotation);
                    Destroy(gameObject);
                }

                break;
            default:
                break;
        }
    }
}
