using UnityEngine;
using System;
using System.Collections.Generic;
using BattleInterface;
using Effects;
using Level;
using Menu;

[RequireComponent(typeof(ItemsCollector))]
[RequireComponent(typeof(ShipMovement))]
[RequireComponent(typeof(Weapon))]
[RequireComponent(typeof(Rigidbody2D))]
public class ShipBehaviour : Destructable
{
    public float dashDuration = 0.5f;
    public float dashAcceleration = 1f;
    public float dashCooldown = 1f;
    
    public float fuelRegeneration = 0.1f;
    
    private InfoPanel infoPanel;

    private ShipMovement movement;
    private int scores = 0;
    private int objectives = 0;
    private float endGamePoint = -1;
    private float fuel;
    public bool isBurningFuel;
    private bool hasShield = false;

    void Awake()
    {   
        infoPanel = Canvas.FindObjectOfType<InfoPanel>();
        movement = GetComponent<ShipMovement>();

        fuel = 0;
        isBurningFuel = false;
        substructLives = false;

        infoPanel.Init();
        UpdateScoresView();

        Init();
    }

    public void StartGame()
    {
        movement.StartGame();
        AddSelectedBoosts(LevelSettings.selectedBoosts);
    }

    private void AddSelectedBoosts(List<Boost> selectedBoosts)
    {
        foreach (Boost boost in selectedBoosts)
        {
            switch (boost.GetBoostType())
            {
                    case Boost.BoostType.Magnet:
                        GetComponent<ItemsCollector>().SetRadius(boost.GetLevel() == 1 ? 
                            ItemsCollector.MID_RADIUS : ItemsCollector.BIG_RADIUS);
                        break;
                    case Boost.BoostType.Fuel:
                        fuelRegeneration += 0.1f;
                        break;
                    case Boost.BoostType.Defence:
                        hasShield = true;
                        break;
            }
        }
    }

    private void UpdateScoresView()
    {
        infoPanel.SetScores(scores);
        infoPanel.UpdateStarsCount();
    }

    public void DashForward()
    {
        if (fuel >= 1f && !isBurningFuel)
        {
            Dash dash = gameObject.AddComponent<Dash>();
            dash.SetAcceleration(dashAcceleration);
            dash.SetDuration(dashDuration);
            dash.SetCooldown(dashCooldown);
        }
    }

    public void SetFuel(float value)
    {
        fuel = Mathf.Max(0, Mathf.Min(1f, value));
        infoPanel.ShowFuel(fuel);
    }

    public float GetFuel()
    {
        return fuel;
    }

    void Update()
    {
        if (!movement.IsStopped() && !isBurningFuel && fuel < 1f)
        {
            SetFuel(fuel + Time.deltaTime * fuelRegeneration);
        }
    }

    public void Shoot()
    {
        if (GetComponent<Weapon>().Shoot(movement.GetBarrelPoint(), movement.GetCurrentRotation()))
        {
            if (anim != null) anim.SetTrigger("Shoot");
            LevelSettings.AddShotsCount(1);
            UpdateScoresView();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag(DESTROYER_TAG)) Die();
        if (coll.gameObject.CompareTag(DAMAGER_TAG)) HitObject(coll.gameObject);
    }

    private void HitObject(GameObject collided)
    {
        GetHit(1);
        
        Destructable target = collided.GetComponent<Destructable>();
        if (target != null) 
        {
            if (!canBeDamaged)
            {
                target.GetHit(target.lives);
            }
            else
            {
                target.AnimateHit();
            }
        }
    }

    public void Collect(GameObject item)
    {
        AddCollectedValue(item.GetComponent<Collectable>());
        Destroy(item);
        UpdateScoresView();
    }

    private void AddCollectedValue(Collectable collectable)
    {
        int bonus = 0;
        if (collectable == null)
        {
            ++bonus;
        }
        else if (collectable.type == Collectable.Type.Score)
        {
            bonus += (int)Math.Round(collectable.value);
        }
        else if (collectable.type == Collectable.Type.Objective)
        {
            objectives += (int)Math.Round(collectable.value);
        }
        else if (collectable.type == Collectable.Type.Speed)
        {
            movement.AccelerateForward(collectable.value);
        }
        scores += bonus;

        LevelSettings.SetScoresCount(scores);
        LevelSettings.SetObjectivesCount(objectives);
        CheckSpeed();
    }

    protected override void OnDamaged(int damage)
    {
        base.OnDamaged(damage);
        UpdateScoresView();
        ChangeSpeed();
    }

    private void ChangeSpeed()
    {
        movement.AccelerateForward(-0.2f);
        CheckSpeed();
    }

    private void CheckSpeed()
    {
        if (movement.IsTooSlow())
        {
            if (hasShield)
            {
                AnimateHit();
                movement.AccelerateForward(0.4f);
                hasShield = false;
                return;
            }
            Invoke("DieIfStillSlow", CameraController.speedAdjustmentTime);
        }
    }

    private void DieIfStillSlow()
    {
        if (movement.IsTooSlow()) 
            GotoMainMenu();
    }

    public override void Die()
    {
        if (hasShield)
        {
            hasShield = false;
            AnimateHit();
            return;
        }
        if (anim != null) base.Die();
        RemoveAllEffects();
        movement.Stop();
        LevelSettings.SaveStarsCountForLevel();
        Invoke("GotoMainMenu", 3f);
    }

    private void RemoveAllEffects()
    {
        TemporalEffect[] effects = GetComponents<TemporalEffect>();
        foreach (TemporalEffect effect in effects)
        {
            Destroy(effect);
        }
    }

    private void GotoMainMenu()
    {
        if (Canvas.FindObjectsOfType<EndLevelWindow>().Length > 0) return;

        movement.Stop();

        infoPanel.ShowEndWnidow();
    }

    public override bool IsAlive()
    {
        return base.IsAlive() && !movement.IsTooSlow();
    }
}
