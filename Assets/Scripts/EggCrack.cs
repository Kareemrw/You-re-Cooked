using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggCrack: MonoBehaviour
{
    [SerializeField] private GameObject crackedEggPrefab; // Prefab for cracked egg contents
    [SerializeField] private float crackForceThreshold = 5f; // Minimum force to crack
    private bool wasThrown = false;

    // Call this when the egg is thrown
    public void MarkAsThrown()
    {
        wasThrown = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the egg was thrown and impact force exceeds threshold
        if (wasThrown && collision.relativeVelocity.magnitude >= crackForceThreshold)
        {
            CrackEgg();
        }
    }

    private void CrackEgg()
    {
        // Spawn cracked egg contents
        Instantiate(crackedEggPrefab, transform.position, transform.rotation);
        Destroy(gameObject); // Remove the whole egg
    }
}