using UnityEngine;

public class SushiRoller : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform[] ingredientSlots; // Slots for seaweed, rice, and salmon
    [SerializeField] private float rollingVelocityThreshold = 2f;
    [SerializeField] private GameObject sushiPrefab;
    [SerializeField] private PlayerPickupDrop playerPickupDrop;
    [SerializeField] private Transform cheeseSpreadChild; // Reference to the "Spread" child
    [SerializeField] private Transform avocadoSpreadChild;
    [SerializeField] private Transform riceChild;

    private bool[] slotFilled;
    private Rigidbody rollerRigidbody;
    private bool isRolling = false;
    private int salmonCounter = 0;

    private void Start()
    {
        rollerRigidbody = GetComponent<Rigidbody>();
        slotFilled = new bool[ingredientSlots.Length];

        // Ensure the spread child is initially inactive
        if (cheeseSpreadChild != null && avocadoSpreadChild != null)
        {
            avocadoSpreadChild.gameObject.SetActive(false);
            cheeseSpreadChild.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ObjectGrabbable foodItem = other.GetComponent<ObjectGrabbable>();
        if (foodItem != null)
        {
            // Handle dropping held ingredients
            if (other.CompareTag("Seaweed") || other.CompareTag("Salmon"))
            {
                if (foodItem.isHeld && playerPickupDrop != null)
                {
                    playerPickupDrop.DropObject();
                }
            }

            // Place ingredients in slots
            if (other.CompareTag("Seaweed") && !slotFilled[0])
            {
                PlaceIngredient(other.transform, 0);
            }
            
            else if (other.CompareTag("Salmon"))
            {
                if (foodItem.isHeld) return;
                for (int slotIndex = 1; slotIndex <= 4; slotIndex++)
                {
                    if (!slotFilled[slotIndex])
                    {
                        slotFilled[slotIndex] = true;
                        PlaceIngredient(other.transform, slotIndex);
                        salmonCounter++;

                        Debug.Log($"Salmon in slot {slotIndex}. Total: {salmonCounter}/4");
                        other.enabled = false;

                        if (salmonCounter >= 4)
                        {
                            Debug.Log("All salmon placed.");
                        }
                        break;
                    }
                }
            }
        }

        // Handle knife with cream cheese or avocado
        if (other.CompareTag("Knife"))
        {
            Transform creamCheese = other.transform.Find("CreamCheese");
            Transform avocado = other.transform.Find("Avocado");

            // Check if CreamCheese is active on the knife
            if (creamCheese != null && creamCheese.CompareTag("CreamCheese") && creamCheese.gameObject.activeSelf)
            {
                ActivateCheeseSpread();
                DeactivateKnifeSpread(other.gameObject, "CreamCheese"); // Deactivate on knife
            }
            // Check if Avocado is active on the knife
            else if (avocado != null && avocado.CompareTag("Avocado") && avocado.gameObject.activeSelf)
            {
                ActivateAvocadoSpread();
                DeactivateKnifeSpread(other.gameObject, "Avocado"); // Deactivate on knife
            }
        }
         if (other.CompareTag("Paddle"))
        {
            Transform rice = other.transform.Find("RiceChild");

            // Check if CreamCheese is active on the knife
            if (rice != null && rice.CompareTag("CookedRice") && rice.gameObject.activeSelf)
            {
                ActivateRice();
                DeactivateKnifeSpread(other.gameObject, "RiceChild"); // Deactivate on knife
            }
        }
    }

    private void ActivateCheeseSpread()
    {
        if (cheeseSpreadChild != null)
        {
            cheeseSpreadChild.gameObject.SetActive(true);
            Debug.Log("Cheese spread activated!");
        }
    }
    private void ActivateRice()
    {
        if (riceChild != null)
        {
            riceChild.gameObject.SetActive(true);
            Debug.Log("Rice activated!");
        }
    }

    private void ActivateAvocadoSpread()
    {
        if (avocadoSpreadChild != null)
        {
            avocadoSpreadChild.gameObject.SetActive(true);
            Debug.Log("Avocado spread activated!");
        }
    }

    private void DeactivateKnifeSpread(GameObject knife, string spreadName)
    {
        Transform spread = knife.transform.Find(spreadName);
        if (spread != null)
        {
            spread.gameObject.SetActive(false);
            Debug.Log($"{spreadName} deactivated on knife.");
        }
    }


    private void PlaceIngredient(Transform ingredient, int slotIndex)
    {
        ingredient.position = ingredientSlots[slotIndex].position;
        ingredient.rotation = ingredientSlots[slotIndex].rotation;

        Collider ingredientCollider = ingredient.GetComponent<Collider>();
        if (ingredientCollider != null)
        {
            ingredientCollider.enabled = false;
        }

        Rigidbody ingredientRigidbody = ingredient.GetComponent<Rigidbody>();
        if (ingredientRigidbody != null)
        {
            Destroy(ingredientRigidbody);
        }

        ingredient.SetParent(ingredientSlots[slotIndex]);
        slotFilled[slotIndex] = true;
    }

    private void Update()
    {
        if (AllSlotsFilled() && !isRolling &&  avocadoSpreadChild.gameObject.activeSelf && riceChild.gameObject.activeSelf &&  cheeseSpreadChild.gameObject.activeSelf)
        {
            if (rollerRigidbody.velocity.magnitude > rollingVelocityThreshold)
            {
                RollSushi();
            }
        }
    }

    private bool AllSlotsFilled()
    {
        foreach (bool filled in slotFilled)
        {
            if (!filled) return false;
        }
        return true;
    }

    private void RollSushi()
    {
        isRolling = true;

        Vector3 sushiPosition = transform.position;
        Quaternion sushiRotation = transform.rotation;
        var copy = sushiPrefab;

        ClearRoller();
        Instantiate(copy, sushiPosition, sushiRotation);
    }

    private void ClearRoller()
    {
        foreach (Transform slot in ingredientSlots)
        {
            if (slot.childCount > 0)
            {
                Destroy(slot.GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < slotFilled.Length; i++)
        {
            slotFilled[i] = false;
        }

        // Reset spread
        if (cheeseSpreadChild != null && avocadoSpreadChild != null)
        {
            avocadoSpreadChild.gameObject.SetActive(false);
            cheeseSpreadChild.gameObject.SetActive(false);
            riceChild.gameObject.SetActive(false);
        }

        isRolling = false;
    }
}