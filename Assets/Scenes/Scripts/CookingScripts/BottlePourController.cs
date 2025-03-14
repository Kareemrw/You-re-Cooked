using UnityEngine;

public class BottlePourController : MonoBehaviour
{
    [SerializeField] private float pourThreshold = 80f;
    [SerializeField] private ParticleSystem pourEffect;
    [SerializeField] private Transform liquidSurface;

    private bool isPouring = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (pourEffect != null) pourEffect.Stop();
    }

    private void Update()
    {
        // Calculate forward tilt angle
        float pourAngle = Vector3.Angle(transform.forward, Vector3.up);
        
        // Should pour if past threshold and not upright
        bool shouldPour = pourAngle > pourThreshold && !IsBottleUpright();

        if (shouldPour && !isPouring)
        {
            StartPour();
        }
        else if (!shouldPour && isPouring)
        {
            StopPour();
        }

        if (liquidSurface != null)
        {
            // Improved liquid surface alignment
            liquidSurface.up = Vector3.Lerp(liquidSurface.up, -transform.forward, Time.deltaTime * 10f);
        }
    }
    
    private void StartPour()
    {
        isPouring = true;
        if (pourEffect != null) pourEffect.Play();
    }

    private void StopPour()
    {
        isPouring = false;
        if (pourEffect != null) pourEffect.Stop();
    }

    private bool IsBottleUpright()
    {
        // Fixed upright check (now checks for true upright position)
        return Vector3.Angle(transform.forward, Vector3.up) < 20f;
    }
}