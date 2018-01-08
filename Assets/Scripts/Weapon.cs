using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject ammo;
    public float cooldown = 3f;

    private float coolingLeft;

    void Update()
    {
        if (IsCooling())
        {
            coolingLeft = Math.Max(coolingLeft - Time.deltaTime, 0);
        }
    }

    public bool Shoot(Vector3 position, float direction)
    {
        if (IsCooling()) return false;
        coolingLeft = cooldown;

        GameObject bullet = Instantiate(ammo);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        bullet.transform.position = position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, direction);

        return true;
    }

    public bool IsActive()
    {
        return ammo != null;
    }

    public bool IsCooling()
    {
        return coolingLeft > 0;
    }
}