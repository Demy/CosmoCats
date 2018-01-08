using UnityEngine;

namespace BattleInterface
{
    public class JoystickController : MonoBehaviour
    {
        private Transform up;
        private Transform down;
        private Vector3 defaultPosition;
        private Camera camera;

        void Start()
        {
            up = transform.GetChild(0);
            down = transform.GetChild(1);

            up.gameObject.SetActive(false);
            down.gameObject.SetActive(false);

            defaultPosition = transform.position;
        
            camera = Camera.main;
        }

        void Update()
        {
            int i = Input.touchCount;
            Touch touch;
            while (i-- > 0)
            {
                touch = Input.GetTouch(i);
                ActOnTouch(touch.position.x, touch.position.y, touch.phase == TouchPhase.Began,
                    touch.phase == TouchPhase.Ended);
            }
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            {
                ActOnTouch(Input.mousePosition.x, Input.mousePosition.y, Input.GetMouseButtonDown(0),
                    Input.GetMouseButtonUp(0));
            }
        }

        private void ActOnTouch(float x, float y, bool isStarted, bool isEnded)
        {
            if (isStarted && x > camera.pixelWidth * 0.66f)
            {
                up.gameObject.SetActive(true);
                down.gameObject.SetActive(true);
                transform.Translate(Input.mousePosition - transform.position);
            }
            if (isEnded)
            {
                up.gameObject.SetActive(false);
                down.gameObject.SetActive(false);
                transform.Translate(defaultPosition - transform.position);
            }
        }
    }
}
