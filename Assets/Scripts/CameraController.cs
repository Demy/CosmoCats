using DigitalRuby.Tween;
using UnityEngine;

public class CameraController
{
    public static float speedAdjustmentTime = 1f;
    
    private const float ZOOM_FACTOR = 3f;
    private const float POSITION_FACTOR = 10f;
    private const float ZERO_SIZE = 6f;
    
    private readonly bool adjustPosition;

    private Camera camera;

    public CameraController(float startVelocity, bool adjustPosition)
    {
        camera = Camera.main;

        this.adjustPosition = adjustPosition;
        
        Resize(0, startVelocity);
    }

    public void Resize(float velocity, float increment)
    {
        float newSize;
        camera.gameObject.Tween("CameraSize", velocity, velocity + increment, speedAdjustmentTime, TweenScaleFunctions.QuadraticEaseOut,
            (t) =>
            {
                newSize = ZERO_SIZE + t.CurrentValue * ZOOM_FACTOR;
                camera.orthographicSize = newSize;
                if (adjustPosition)
                {
                    float newPosition = GetScreenSidePosition(newSize) - 
                                        (t.CurrentValue - ShipMovement.MIN_SPEED) * POSITION_FACTOR;
                    if (t.CurrentValue <= ShipMovement.MIN_SPEED) newPosition += 0.1f * POSITION_FACTOR;
                    camera.transform.Translate(newPosition - camera.transform.localPosition.x, 0f, 0f);
                }
            },
            (t) => {  });
    }

    private float GetScreenSidePosition(float newSize)
    {
        return (newSize - 2f) * 2f;
    }

    public void Stop()
    {
    }
}