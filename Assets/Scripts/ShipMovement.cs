using System;
using DigitalRuby.Tween;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public const float MIN_SPEED = 0.25f;
    
    private float verticalStep = 3.7f;

    public float inertiaTime = 0.3f;
    public float hVelocity = 0.3f;
    public float rotationSpeed = 1.5f;
    
    private CameraController cameraController;

    private int currentPosition;

    private Vector2 velocity;

    private Transform sprite;
    private Transform barrelPoint;
    
    private bool isControlled = true;

    private bool isStopped;

    void Start()
    {
        currentPosition = 0;

        velocity = new Vector2();
        ChangeVerticalPosition(currentPosition);

        cameraController = new CameraController(hVelocity, true);

        isStopped = true;

        sprite = transform.Find("Ship");
        barrelPoint = sprite.Find("BarrelPoint");
    }

    public void StartGame()
    {
        isStopped = false;
    }

    private void FixedUpdate()
    {
        if (isStopped) return;

        velocity.Set(hVelocity, 0);
        transform.Translate(velocity);

        RotateSprite(velocity);
    }

    private void RotateSprite(Vector3 velocity)
    {
        sprite.Rotate(0f, 0f, 180f * (float)(Math.Atan2(velocity.y, velocity.x) / Math.PI - sprite.rotation.z) *
                              Time.deltaTime * rotationSpeed);
    }

    public void AccelerateForward(float byValye = 0f)
    {
        cameraController.Resize(hVelocity, byValye);
        hVelocity = Math.Max(0, hVelocity + byValye);
    }

    public void AccelerateBy(float value)
    {
        value = Math.Max(-1f, Math.Min(1f, value));
        if (!isStopped) ChangeVerticalPositionBy(value);
    }

    private void ChangeVerticalPositionBy(float value)
    {
        gameObject.Tween("ShipMovementY", 0, verticalStep * value * 0.2f, 0.1f, TweenScaleFunctions.Linear,
            (t) => { transform.Translate(0f, t.CurrentValue, 0f); },
            (t) => {  });
    }

    public void AccelerateUp()
    {
        if (!isStopped) ChangeVerticalPosition(1);
    }

    public void AccelerateDown()
    {
        if (!isStopped) ChangeVerticalPosition(-1);
    }

    private void ChangeVerticalPosition(int step)
    {
        if (!isControlled) return;

        float oldPosition = currentPosition;
        currentPosition = Math.Min(1, Math.Max(-1, currentPosition + step));
        float time = inertiaTime * Math.Abs(currentPosition - (transform.position.y / verticalStep));
        if (oldPosition != currentPosition)
        {
            gameObject.Tween("ShipMovementY", transform.position.y, currentPosition * verticalStep, 
                time, 
                TweenScaleFunctions.Linear,
                (t) => { transform.Translate(0f, t.CurrentValue - transform.position.y, 0f); },
                (t) => { SetCanMove(); });
            isControlled = false;
        }
    }

    private void SetCanMove()
    {
        isControlled = true;
    }

    public void Stop()
    {
        isStopped = true;
        cameraController.Stop();
    }

    public Vector3 GetBarrelPoint()
    {
        return barrelPoint.position;
    }

    public float GetCurrentRotation()
    {
        return sprite.rotation.eulerAngles.z;
    }

    public bool IsTooSlow()
    {
        return hVelocity <= MIN_SPEED;
    }

    public bool IsStopped()
    {
        return isStopped;
    }
}
