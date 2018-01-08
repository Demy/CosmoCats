using UnityEngine;

namespace BattleInterface
{
    public class TouchConrtoller : MonoBehaviour
    {
        private Camera _camera;
    
        public enum ControlOption
        {
            TapToDash, SlideToDash, Original
        }

        private ShipMovement ship;
        private ShipBehaviour player;

        private ControlOption currentControlOption;

        private float startPosition;

        void Start()
        {
            ship = GetComponent<ShipMovement>();
            player = GetComponent<ShipBehaviour>();
        
            _camera = Camera.main;
        }

        void Update()
        {
            if (player.IsAlive())
            {
                int i = Input.touchCount;
                Touch touch;
                while (i-- > 0)
                {
                    touch = Input.GetTouch(i);
                    ActOnTouch(touch.position.x, touch.position.y, touch.phase);
                }
                if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
                {
                    ActOnTouch(Input.mousePosition.x, Input.mousePosition.y, GetMouseToucPhase());
                } 
            }
        }

        private TouchPhase GetMouseToucPhase()
        {
            if (Input.GetMouseButtonUp(0)) return TouchPhase.Ended;
            if (Input.GetMouseButtonDown(0)) return TouchPhase.Began;
            if (Input.mousePosition.y != startPosition) return TouchPhase.Moved;
            return TouchPhase.Stationary;
        }

        private void ActOnTouch(float x, float y, TouchPhase phase)
        {
            if (player.lives <= 0 || ship.IsStopped()) return;
        
            if (x > _camera.pixelWidth * 0.5f)
            {
                ApplyCurrentControlOption(y, phase);
            }
            else if (x < _camera.pixelWidth * 0.5f && y <= _camera.pixelHeight * 0.5f)
            {
                player.Shoot();
            }
            else if (x < _camera.pixelWidth * 0.5f && y > _camera.pixelHeight * 0.5f)
            {
                player.DashForward();
            }
        }

        private void ApplyCurrentControlOption(float positionY, TouchPhase phase)
        {
            if (phase == TouchPhase.Began)
                startPosition = positionY;

            switch (currentControlOption)
            {
                case ControlOption.TapToDash:
                    ActOnTap(positionY, phase);
                    break;
                case ControlOption.SlideToDash:
                    ActOnSlide(positionY, phase);
                    break;
                default:
                    ActOriginal(positionY, phase);
                    break;
            }
        }

        private void ActOnTap(float positionY, TouchPhase phase)
        {
            if (phase == TouchPhase.Began)
            {
                if (positionY < _camera.pixelHeight * 0.5f)
                {
                    ship.AccelerateDown();
                }
                else
                {
                    ship.AccelerateUp();
                }
            }
        }

        private void ActOnSlide(float positionY, TouchPhase phase)
        {
            if (phase == TouchPhase.Ended)
            {
                if (startPosition - positionY > 0)
                {
                    ship.AccelerateDown();
                }
                else if (startPosition - positionY < 0)
                {
                    ship.AccelerateUp();
                }
            }
        }

        private void ActOriginal(float positionY, TouchPhase phase)
        {
            float tapPosition = (startPosition - positionY) / (_camera.pixelHeight * 0.25f);
            ship.AccelerateBy(-tapPosition);
        }
    }
}