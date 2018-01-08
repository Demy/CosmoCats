using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float MAX_LIFETIME = 5f;

    public float speed;
    public int damage = 1;

    void Start()
    {
        Invoke("Die", MAX_LIFETIME);
    }

    void Update()
    {
        transform.Translate(speed, 0, 0, Space.Self);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == Collectable.BONUS_TAG) return;

        Destructable target = coll.gameObject.GetComponent<Destructable>();

        if (target != null)
        {
            if (target.IsAlive())
            {
                target.GetHit(damage);
                Explode();
            }
        }
        else
        {
            Explode();
        }
    }

    private void Explode()
    {
        Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}