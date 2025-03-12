using UnityEngine;

public class EggCrack : MonoBehaviour
{
    [SerializeField] private GameObject crackedEggPrefab; 
    [SerializeField] private float crackForceThreshold = 5f;
    [SerializeField] private AudioClip crackSound;
    [SerializeField] private float maxHearingDistance = 20f; // Maximum distance sound can be heard
    
    private bool wasThrown = false;
    private Transform playerTransform;

    private void Start()
    {
        // Find the player transform (assign directly in Inspector if possible)
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

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
        if (playerTransform != null)
        {
            // Calculate distance to player
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            
            // Calculate volume based on distance (linear falloff)
            float volume = Mathf.Clamp01(1 - (distance / maxHearingDistance));
            
            // Add a minimum volume so it's never completely silent
            volume = Mathf.Clamp(volume, 0.1f, 1f);
            
            SoundFXManager.instance.PlaySoundFXClip(crackSound, transform, volume);
        }
        else
        {
            // Fallback if player not found
            SoundFXManager.instance.PlaySoundFXClip(crackSound, transform, 1f);
        }

        Instantiate(crackedEggPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}