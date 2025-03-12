using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeChop : MonoBehaviour
{
    [SerializeField] private GameObject wholeIngredient; 
    [SerializeField] private GameObject choppedPrefab; 
    [SerializeField] private float chopForceThreshold = 5f; 
    [SerializeField] private int numberOfPieces = 4;
    [SerializeField] private AudioClip chopSound;
    [SerializeField] private float maxHearingDistance = 20f;
    private Transform playerTransform;

    public bool isChopped = false;

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
         if (playerTransform != null)
        {
            // Calculate distance to player
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            
            // Calculate volume based on distance (linear falloff)
            float volume = Mathf.Clamp01(1 - (distance / maxHearingDistance));
            
            // Add a minimum volume so it's never completely silent
            volume = Mathf.Clamp(volume, 0.1f, 1f);
            
            SoundFXManager.instance.PlaySoundFXClip(chopSound, transform, volume);
        }
        else
        {
            // Fallback if player not found
            SoundFXManager.instance.PlaySoundFXClip(chopSound, transform, 1f);
        }
        isChopped = true;

        Vector3 ingredientPosition = wholeIngredient.transform.position;
        Quaternion ingredientRotation = wholeIngredient.transform.rotation;
        
        Renderer ingredientRenderer = wholeIngredient.GetComponent<Renderer>();
        if (ingredientRenderer == null) return; 
        float ingredientDepth = ingredientRenderer.bounds.size.z; 
        float sliceSpacing = ingredientDepth / (numberOfPieces + 1) * 1.1f;

        Destroy(wholeIngredient);

        for (int i = 0; i < numberOfPieces; i++)
        {
            float offsetZ = (-ingredientDepth / 2) + (i + 1) * sliceSpacing; 
            Vector3 piecePosition = ingredientPosition + new Vector3(0, 0, offsetZ);
            
            Instantiate(choppedPrefab, piecePosition, ingredientRotation);
        }
    }
}