using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggCrack: MonoBehaviour
{
    [SerializeField] private GameObject crackedEggPrefab; 
    [SerializeField] private float crackForceThreshold = 5f;
    private bool wasThrown = false;

    // Call this when the egg is thrown
    public void MarkAsThrown()
    {
        wasThrown = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (wasThrown && collision.relativeVelocity.magnitude >= crackForceThreshold)
        {
            CrackEgg();
        }
    }

    private void CrackEgg()
    {
        Instantiate(crackedEggPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}