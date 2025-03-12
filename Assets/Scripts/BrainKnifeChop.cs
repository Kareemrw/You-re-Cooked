using UnityEngine;

public class BrainKnifeChop : MonoBehaviour
{
    [SerializeField] private GameObject wholeIngredient;
    [SerializeField] private GameObject choppedPiece1; // First half
    [SerializeField] private GameObject choppedPiece2; // Second half
    [SerializeField] private float chopForceThreshold = 5f;
    [SerializeField] private float pieceSeparation = 0.2f; // Distance between pieces
     [SerializeField] private AudioClip chopSound;
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
        SoundFXManager.instance.PlaySoundFXClip(chopSound, transform, 1f);
        isChopped = true;
        Vector3 ingredientPosition = wholeIngredient.transform.position;
        Quaternion ingredientRotation = wholeIngredient.transform.rotation;

        // Destroy the whole ingredient
        Destroy(wholeIngredient);

        // Calculate positions for two pieces
        Vector3 piece1Position = ingredientPosition + new Vector3(-pieceSeparation, 0, 0);
        Vector3 piece2Position = ingredientPosition + new Vector3(pieceSeparation, 0, 0);

        // Instantiate both pieces
        Instantiate(choppedPiece1, piece1Position, ingredientRotation);
        Instantiate(choppedPiece2, piece2Position, ingredientRotation);
    }
}