using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [Header("Slot Transforms")]
    [SerializeField] private Transform[] rollSlots = new Transform[7];    // 7 sushi roll slots
    [SerializeField] private Transform balutSlot;    
    [SerializeField] private Transform soySauceSlot;       
    [SerializeField] private Transform vinegarSlot;                 // 1 balut slot
    [SerializeField] private Transform[] headSlots = new Transform[2];    // 2 head slice slots

    [Header("Placement Settings")]
    [SerializeField] private float verticalOffset = 0.1f;  // Vertical spacing between items

    // Track occupied slots
    private bool[] rollSlotFilled = new bool[7];
    private bool balutSlotFilled = false;
    private bool soySauceSlotFilled = false;
    private bool vinegarSlotFilled = false;
    private bool[] headSlotFilled = new bool[2];

    // Completion flags
    public bool SushiComplete = false;
    public bool BalutComplete = false;
    public bool GoatComplete = false;
    [SerializeField] private PlayerPickupDrop playerPickupDrop;

    private void OnTriggerEnter(Collider other)
    {
        ObjectGrabbable foodItem = other.GetComponent<ObjectGrabbable>();
        if (foodItem != null)
        {
            // Handle dropping held ingredients
            if (other.CompareTag("SushiRoll") || other.CompareTag("Balut") || other.CompareTag("HeadSlice") || other.CompareTag("SoySauceBowl") || other.CompareTag("VinegarBowl"))
            {
                if (foodItem.isHeld && playerPickupDrop != null)
                {
                    playerPickupDrop.DropObject();
                }
            }
        }

        // Handle different ingredient types
        if (other.CompareTag("SushiRoll"))
        {
            TryPlaceInRollSlot(other.gameObject);
        }
        else if (other.CompareTag("Balut"))
        {
            TryPlaceInBalutSlot(other.gameObject);
        }
        else if (other.CompareTag("HeadSlice"))
        {
            TryPlaceInHeadSlot(other.gameObject);
        }
        else if (other.CompareTag("SoySauceBowl"))
        {
            TryPlaceInSoySauceSlot(other.gameObject);
        }
        else if (other.CompareTag("VinegarBowl"))
        {
            TryPlaceInVinegarSlot(other.gameObject);
        }

        // Check for completion after placing the ingredient
        CheckForCompletion();
    }

    private void TryPlaceInRollSlot(GameObject roll)
    {
        for (int i = 0; i < rollSlots.Length; i++)
        {
            if (!rollSlotFilled[i])
            {
                PlaceIngredient(roll, rollSlots[i], Quaternion.Euler(90, 0, 0));
                Debug.Log("slot " + i + " filled");
                rollSlotFilled[i] = true;
                break;
            }
        }
    }

    private void TryPlaceInSoySauceSlot(GameObject soySauce)
    {
        if (!soySauceSlotFilled)
        {
            PlaceIngredient(soySauce, soySauceSlot, Quaternion.identity);
            soySauceSlotFilled = true;
        }
    }

    private void TryPlaceInVinegarSlot(GameObject vinegar)
    {
        if (!vinegarSlotFilled)
        {
            PlaceIngredient(vinegar, vinegarSlot, Quaternion.identity);
            vinegarSlotFilled = true;
        }
    }

    private void TryPlaceInBalutSlot(GameObject balut)
    {
        if (!balutSlotFilled)
        {
            PlaceIngredient(balut, balutSlot, Quaternion.identity);
            balutSlotFilled = true;
        }
    }

    private void TryPlaceInHeadSlot(GameObject head)
    {
        for (int i = 0; i < headSlots.Length; i++)
        {
            if (!headSlotFilled[i])
            {
                PlaceIngredient(head, headSlots[i], Quaternion.identity);
                headSlotFilled[i] = true;
                break;
            }
        }
    }

    private void PlaceIngredient(GameObject ingredient, Transform slot, Quaternion rotation)
    {
        // Set position and rotation relative to the slot
        ingredient.transform.position = slot.position;
        ingredient.transform.rotation = slot.rotation * rotation;

        // Disable collider
        Collider ingredientCollider = ingredient.GetComponent<Collider>();
        if (ingredientCollider != null)
        {
            ingredientCollider.enabled = false;
        }

        // Disable or destroy Rigidbody
        Rigidbody ingredientRigidbody = ingredient.GetComponent<Rigidbody>();
        if (ingredientRigidbody != null)
        {
            Destroy(ingredientRigidbody);
        }

        // Parent the ingredient to the slot
        ingredient.transform.SetParent(slot);

        // Disable Colliders child if it exists
        Transform collidersChild = ingredient.transform.Find("Colliders");
        if (collidersChild != null)
        {
            collidersChild.gameObject.SetActive(false);
        }
    }

    private void ClearSlot(Transform slot)
    {
        if (slot.childCount > 0)
        {
            Destroy(slot.GetChild(0).gameObject);
        }
    }

    public void ClearAll()
    {
        // Clear roll slots
        foreach (var slot in rollSlots)
        {
            ClearSlot(slot);
        }
        rollSlotFilled = new bool[7];

        // Clear balut slot
        ClearSlot(balutSlot);
        balutSlotFilled = false;

        // Clear head slots
        foreach (var slot in headSlots)
        {
            ClearSlot(slot);
        }
        headSlotFilled = new bool[2];

        // Reset completion flags
        SushiComplete = false;
        BalutComplete = false;
    }

    private void CheckForCompletion()
    {
        // Check if all roll slots and soy sauce slot are filled
        bool allRollSlotsFilled = true;
        foreach (bool filled in rollSlotFilled)
        {
            if (!filled)
            {
                allRollSlotsFilled = false;
                break;
            }
        }

        if (allRollSlotsFilled && soySauceSlotFilled)
        {
            SushiComplete = true;
            Debug.Log("Sushi Complete!");
        }

        // Check if balut slot and vinegar slot are filled
        if (balutSlotFilled && vinegarSlotFilled)
        {
            BalutComplete = true;
            Debug.Log("Balut Complete!");
        }
    }

    // Public getters for completion flags
    public bool IsSushiComplete()
    {
        return SushiComplete;
    }

    public bool IsBalutComplete()
    {
        return BalutComplete;
    }
}