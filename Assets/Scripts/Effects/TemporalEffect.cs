using UnityEngine;

namespace Effects
{
    public class TemporalEffect : MonoBehaviour
    {
        protected float duration = -1;
        protected float cooldown = -1;
        protected bool isRunning = false;
    
        private float startTime = -1;
        private float timePassed = -1;

        protected ShipMovement movement;
        protected ShipBehaviour behaviour;

        protected virtual void OnEnable()
        {
            movement = GetComponent<ShipMovement>();
            behaviour = GetComponent<ShipBehaviour>();

            StartEffect();
        }

        protected virtual void OnDisable()
        {
            if (isRunning)
            {
                Remove();
                isRunning = false;
                CancelInvoke("EndEffect");
                timePassed = Time.time - startTime;
            }
        }

        public void SetDuration(float value)
        {
            duration = value;
            StartEffect();
        }

        public void SetCooldown(float cooldown)
        {
            this.cooldown = cooldown;
        }

        protected void StartEffect()
        {
            if (!isRunning && duration > 0)
            {
                StartTimer();
                Apply();
            }
        }

        protected void StartTimer()
        {
            isRunning = true;
            startTime = Time.time;
            Invoke("EndEffect", timePassed > 0 ? timePassed : duration);
        }

        protected virtual void Apply() {}

        protected virtual void Remove() {}

        protected void EndEffect()
        {
            Remove();
            isRunning = false;
            duration = -1;
            startTime = -1;
            timePassed = -1;
            if (cooldown > 0)
            {
                Invoke("EndDuration", cooldown);
            }
            else
            {
                EndDuration();
            }
        }

        private void EndDuration()
        {
            Destroy(this);  
        }
    }
}