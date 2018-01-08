using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class Enemy : Destructable
{
    public float range = 20f;
    public bool followPlayer = false;
    public float verticalSpeed = 1f;
    public float horizontalSpeed = 0.5f;
    public bool keepSameSpeed = false;

    private Weapon gun;
    private Destructable target;
    private bool pathLooping = true;
    
    private bool isOnPosition;

    void Start()
    {
        isOnPosition = true;
        gun = GetComponent<Weapon>();
        target = FindObjectOfType<ShipBehaviour>();
    }

    void Update()
    {
        float distance = (transform.position.x - target.transform.position.x);
        if (distance < -range) Destroy(gameObject);
        if (isOnPosition && followPlayer &&
            ((keepSameSpeed && distance <= range) || distance <= -range * 0.5f))
        {
            StartFollowing();
        }
        if (distance <= range && target.IsAlive())
        {
            if (followPlayer && (keepSameSpeed || distance <= -range * 0.5f)) Follow(target.transform.position);
            if (gun.IsActive() && !gun.IsCooling()) Shoot();
        }
    }

    private void Follow(Vector3 targetPosition)
    {
        if (pathLooping) DisablePath();
        if (isOnPosition) StartFollowing();
        
        float speed = keepSameSpeed ? target.GetComponent<ShipMovement>().hVelocity : horizontalSpeed;
        //transform.Translate((targetPosition.x + range * 0.5f - transform.position.x) * speed * Time.deltaTime,
        //    (targetPosition.y - transform.position.y) * verticalSpeed * Time.deltaTime, 0);
        transform.Translate(speed, 0f, 0f);
    }

    private void StartFollowing()
    {
        transform.SetParent(target.transform.parent);
    }

    private void DisablePath()
    {
        pathLooping = false;
        SplineController path = GetComponent<SplineController>();
        if (path != null)
        {
            path.enabled = false;
            GetComponent<SplineInterpolator>().enabled = false;
        }
    }

    private void Shoot()
    {
        gun.Shoot(transform.position, 180 + transform.rotation.eulerAngles.z);
    }
}