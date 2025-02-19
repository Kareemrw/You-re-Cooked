using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeChop : MonoBehaviour
{
    [SerializeField] private GameObject wholeIngredient; 
    [SerializeField] private GameObject choppedPrefab; 
    [SerializeField] private float chopForceThreshold = 5f; 
    [SerializeField] private int numberOfPieces = 4;


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