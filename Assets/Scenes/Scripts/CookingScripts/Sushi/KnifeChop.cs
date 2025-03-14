using UnityEngine;

public class KnifeChop : MonoBehaviour
{
    [Header("Chopping Settings")]
    [SerializeField] private GameObject wholeIngredient;
    [SerializeField] private GameObject choppedPrefab;
    [SerializeField] private float chopForceThreshold = 5f;
    [SerializeField] private int numberOfPieces = 4;
    [SerializeField] private AudioClip chopSound;
    [SerializeField] private float maxHearingDistance = 20f;

    private Transform playerTransform;
    private Renderer ingredientRenderer;
    public bool isChopped = false;

    private void Start()
    {
        // Cache the Renderer component
        ingredientRenderer = wholeIngredient.GetComponent<Renderer>();

        // Cache the player transform
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Knife"))
        {
            ObjectGrabbable knifeGrabbable = collision.gameObject.GetComponent<ObjectGrabbable>();
            if (knifeGrabbable != null && knifeGrabbable.isHeld && !isChopped)
            {
                if (collision.relativeVelocity.magnitude > chopForceThreshold)
                {
                    ChopIngredient();
                }
            }
        }
    }

    private void ChopIngredient()
    {
        if (isChopped) return; // Prevent multiple chops

        isChopped = true;

        // Play chop sound with distance-based volume
        PlayChopSound();

        // Get ingredient position and rotation
        Vector3 ingredientPosition = wholeIngredient.transform.position;
        Quaternion ingredientRotation = wholeIngredient.transform.rotation;

        // Calculate piece positions
        if (ingredientRenderer != null)
        {
            float ingredientDepth = ingredientRenderer.bounds.size.z;
            float sliceSpacing = ingredientDepth / (numberOfPieces + 1) * 1.1f;

            // Destroy the whole ingredient
            Destroy(wholeIngredient);

            // Spawn chopped pieces
            for (int i = 0; i < numberOfPieces; i++)
            {
                float offsetZ = (-ingredientDepth / 2) + (i + 1) * sliceSpacing;
                Vector3 piecePosition = ingredientPosition + new Vector3(0, 0, offsetZ);
                Instantiate(choppedPrefab, piecePosition, ingredientRotation);
            }
        }
    }

    private void PlayChopSound()
    {
        if (playerTransform != null && SoundFXManager.instance != null)
        {
            // Use sqrMagnitude for better performance
            float distanceSqr = (transform.position - playerTransform.position).sqrMagnitude;
            float maxDistanceSqr = maxHearingDistance * maxHearingDistance;

            // Calculate volume based on distance (linear falloff)
            float volume = Mathf.Clamp01(1 - (distanceSqr / maxDistanceSqr));
            volume = Mathf.Clamp(volume, 0.1f, 1f);

            SoundFXManager.instance.PlaySoundFXClip(chopSound, transform, volume);
        }
        else
        {
            // Fallback if player not found
            SoundFXManager.instance.PlaySoundFXClip(chopSound, transform, 1f);
        }
    }
}