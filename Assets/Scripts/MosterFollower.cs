using System;
using System.Collections;
using UnityEngine;

public class MosterFollower : Destructable
{
    [SerializeField]
    private float velocity = 0.4f;
    
    private float reach = 4f;
    
    private ShipMovement ship;
    private bool isFollowing;
    
    private void Start()
    {
        isFollowing = false;
        ship = FindObjectOfType<ShipMovement>();
        StartCoroutine("WaitForShip");
    }

    private IEnumerator WaitForShip()
    {
        while (ship.transform.position.x < transform.position.x + reach)
            yield return null;
        StartFollowing();
    }

    private void StartFollowing()
    {
        transform.SetParent(ship.transform.parent);
        
        foreach (SpriteRenderer rederer in GetComponentsInChildren<SpriteRenderer>())
        {
            rederer.flipX = true;  
        }
        
        velocity = ship.hVelocity;
        isFollowing = true;
        StartCoroutine("KillIfTooFarAway");
    }

    private IEnumerator KillIfTooFarAway()
    {
        while (ship.transform.position.x < transform.position.x + reach * 3)
            yield return null;
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (isFollowing)
        {
            velocity = Math.Min(ship.hVelocity, velocity);
            float newPos = Math.Min(ship.transform.position.x - reach, transform.position.x + velocity);
            transform.Translate(newPos - transform.position.x, 0f, 0f);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(DESTROYER_TAG)) Die();
        if (coll.gameObject.CompareTag(DAMAGER_TAG)) SlowDown();
    }

    private void SlowDown()
    {
        velocity -= 0.2f;
    }
}