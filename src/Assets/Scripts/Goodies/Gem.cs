using UnityEngine;

public class Gem : MonoBehaviour
{
    public GameState GameState;
    public ParticleSystem glow;
    public ParticleSystem getGlow;

    private SpriteRenderer sprite;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            AddGemCount();
            Instantiate(this.getGlow, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public virtual void AddGemCount()
    { }

    public void OnContainingIceDestroyed()
    {
        glow.Play();
    }
}
