using UnityEngine;

public class SoySauceBowl : MonoBehaviour
{
    [SerializeField] private GameObject soySauceObject; // Reference to the soy sauce visual
    [SerializeField] private int requiredHits = 10; // Number of particle hits needed

    private int currentHits;
    private bool sauceActivated;

    private void Start()
    {
        if (soySauceObject != null)
        {
            soySauceObject.SetActive(false);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        // Check if colliding particles are from a SoySauce system
        if (other.CompareTag("SoySauce") && !sauceActivated)
        {
            currentHits++;
            
            if (currentHits >= requiredHits)
            {
                ActivateSoySauce();
            }
        }
    }

    private void ActivateSoySauce()
    {
        sauceActivated = true;
        if (soySauceObject != null)
        {
            soySauceObject.SetActive(true);
        }
    }
}