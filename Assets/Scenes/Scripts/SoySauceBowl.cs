using UnityEngine;

public class SoySauceBowl : MonoBehaviour
{
    [SerializeField] private GameObject soySauceObject; // Reference to the soy sauce visual
    [SerializeField] private GameObject vinegarObject;
    [SerializeField] private GameObject onionObject;
    [SerializeField] private int requiredHits = 10; // Number of particle hits needed
    [SerializeField] private PlayerPickupDrop playerPickupDrop;

    private int currentHits;
    private bool sauceActivated;
    private bool onionActivated;

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
        if (other.CompareTag("SoySauce") && !sauceActivated && !onionActivated)
        {
            currentHits++;
            
            if (currentHits >= requiredHits)
            {
                ActivateSoySauce();
            }
        }
        else if (other.CompareTag("Vinegar") && !sauceActivated)
        {
            currentHits++;

            if (currentHits >= requiredHits)
            {
                ActivateVinegar();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
         ObjectGrabbable foodItem = other.GetComponent<ObjectGrabbable>();
        if (foodItem != null)
        {
            // Handle dropping held ingredients
            if (other.CompareTag("SlicedOnion"))
            {
                if (foodItem.isHeld && playerPickupDrop != null)
                {
                    playerPickupDrop.DropObject();
                }
            }
        }
         if (other.CompareTag("SlicedOnion") && !onionActivated)
        {
                Destroy(other.gameObject);
                ActivateOnion();
        }
        if(onionObject.activeSelf && vinegarObject.activeSelf)
        {
            gameObject.tag = "VinegarBowl";
        }
    }

    private void ActivateSoySauce()
    {
        sauceActivated = true;
        if (soySauceObject != null)
        {
            soySauceObject.SetActive(true);
        }
        gameObject.tag = "SoySauceBowl";
    }
    private void ActivateVinegar()
    {
        sauceActivated = true;
        if (vinegarObject != null)
        {
            vinegarObject.SetActive(true);
        }
        gameObject.tag = "VinegarBowl";
    }
    private void ActivateOnion()
    {
        onionActivated = true;
        if (onionObject != null)
        {
            onionObject.SetActive(true);
        }
    }
}