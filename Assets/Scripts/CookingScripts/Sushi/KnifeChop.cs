using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeChop : MonoBehaviour
{
    [SerializeField] private GameObject wholeIngredient; 
    [SerializeField] private GameObject choppedPrefab; 
    [SerializeField] private float chopForceThreshold = 5f; 
    [SerializeField] private int numberOfPieces = 4;
    [SerializeField] private Vector3 pieceSpacing = new Vector3(0.1f, 0, 0.1f); 

    private bool isChopped = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isChopped && collision.gameObject.CompareTag("Knife"))
        {
            if (collision.relativeVelocity.magnitude > chopForceThreshold)
            {
                ChopIngredient();
            }
        }
    }

    private void ChopIngredient()
    {
        isChopped = true;

        Vector3 ingredientPosition = wholeIngredient.transform.position;
        Quaternion ingredientRotation = wholeIngredient.transform.rotation;

        // Destroy the whole ingredient
        Destroy(wholeIngredient);

        for (int i = 0; i < numberOfPieces; i++)
        {
            Vector3 piecePosition = ingredientPosition + new Vector3(
                i % 2 * pieceSpacing.x, 0, i / 2 * pieceSpacing.z 
            );
            Instantiate(choppedPrefab, piecePosition, ingredientRotation);
        }

        Debug.Log($"🔪 {wholeIngredient.name} chopped into {numberOfPieces} pieces!");
    }
}