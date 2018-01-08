using System;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    protected const String DESTROYER_TAG = "Destroyer";
    protected const String DAMAGER_TAG = "Damager";
    
    public int lives = 3;

    protected Animator anim;

    protected bool canBeDamaged = true;
    protected bool substructLives = true;

    void Start()
    {
        Init();
    }

    protected void Init()
    {
        anim = GetComponent<Animator>();
    }

    public virtual void GetHit(int damage)
    {
        if (canBeDamaged)
        {
            OnDamaged(damage);
        }
    }

    protected virtual void OnDamaged(int damage)
    {
        if (substructLives) lives -= damage;

        if (lives <= 0)
        {
            Die();
        }
        else
        {
            AnimateHit();
        }
    }

    public virtual void AnimateHit()
    {
        if (anim != null) anim.SetTrigger("Hit");
    }

    public virtual void AnimateDeath()
    {
        if (anim != null) anim.SetTrigger("Die");
    }

    public virtual void Die()
    {
        if (anim != null)
        {
            AnimateDeath();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public virtual bool IsAlive()
    {
        return lives > 0;
    }

    public void SetCanBeDamaged(bool value)
    {
        canBeDamaged = value;
    }
}