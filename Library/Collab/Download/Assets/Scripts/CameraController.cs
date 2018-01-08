using DigitalRuby.Tween;
using UnityEngine;

public class CameraController
{
    public static float speedAdjustmentTime = 1f;
    
    private const float ZOOM_FACTOR = 5f;
    private const float POSITION_FACTOR = 20f;
    private const float ZERO_SIZE = 6f;
    
    private readonly bool adjustPosition;

    private Camera camera;

    public CameraController(float startVelocity, bool adjustPosition)
    {
        camera = Camera.main;

        this.adjustPosition = adjustPosition;
        
        Resize(startVelocity);
    }

    public void Resize(float newVelocity)
    {
        float newSize = ZERO_SIZE + newVelocity * ZOOM_FACTOR;
        camera.gameObject.Tween("CameraSize", camera.orthographicSize, newSize, speedAdjustmentTime, TweenScaleFunctions.QuadraticEaseOut,
            (t) => { camera.orthographicSize = t.CurrentValue; },
            (t) => {  });

        if (adjustPosition) AdjustPositionBySpeed(newVelocity, newSize);
    }

    private void AdjustPositionBySpeed(float newVelocity, float newSize)
    {
        float newPosition = GetScreenSidePosition(newSize) - (newVelocity - ShipMovement.MIN_SPEED) * POSITION_FACTOR;
        if (newVelocity <= ShipMovement.MIN_SPEED) newPosition += 0.1f * POSITION_FACTOR;
        
       // camera.gameObject.Tween("CameraMove", 0, newPosition - camera.transform.position.x, speedAdjustmentTime, 
       //     TweenScaleFunctions.QuadraticEaseOut,
       //     (t) => { camera.transform.Translate(t.CurrentValue, 0f, 0f); },
       //     (t) => {  });
    }

    private float GetScreenSidePosition(float newSize)
    {
        return (newSize - 2f) * 2f;
    }

    public void Stop()
    {
    }
}