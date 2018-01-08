using UnityEngine;

namespace Effects
{
    public class Dash : TemporalEffect
    {
        private float _acceleration = 0;
    
        protected override void Apply()
        {
            behaviour.isBurningFuel = true;
            movement.AccelerateForward(_acceleration);
            behaviour.SetCanBeDamaged(false);
        }

        protected override void Remove()
        {
            movement.AccelerateForward(-_acceleration);
            behaviour.SetCanBeDamaged(true);
            behaviour.isBurningFuel = false;
        }

        public void SetAcceleration(float value)
        {
            _acceleration = value;
        }

        void Update()
        {
            behaviour.SetFuel(behaviour.GetFuel() - Time.deltaTime / duration);
        }
    }
}