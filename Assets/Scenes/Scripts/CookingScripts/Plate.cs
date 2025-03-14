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
    [SerializeField] private Transform headSlot1;                   // First head slot
    [SerializeField] private Transform headSlot2;                   // Second head slot

    [Header("Placement Settings")]
    [SerializeField] private float verticalOffset = 0.1f;  // Vertical spacing between items

    // Track occupied slots
    private bool[] rollSlotFilled = new bool[7];
    private bool balutSlotFilled = false;
    private bool soySauceSlotFilled = false;
    private bool vinegarSlotFilled = false;
    private bool headSlot1Filled = false;
    private bool headSlot2Filled = false;

    // Completion flags
    public bool SushiComplete = false;
    public bool BalutComplete = false;
    public bool GoatComplete = false;

    // Group management
    private enum IngredientGroup { None, Sushi, Balut, Goat }
    private IngredientGroup activeGroup = IngredientGroup.None;

    [SerializeField] private PlayerPickupDrop playerPickupDrop;

    private void OnTriggerEnter(Collider other)
    {
        ObjectGrabbable foodItem = other.GetComponent<ObjectGrabbable>();
        if (foodItem != null)
        {
            // Handle dropping held ingredients
            if (other.CompareTag("SushiRoll") || other.CompareTag("Balut") || 
                other.CompareTag("HeadSlice1") || other.CompareTag("HeadSlice2") || 
                other.CompareTag("SoySauceBowl") || other.CompareTag("VinegarBowl"))
            {
                if (foodItem.isHeld && playerPickupDrop != null)
                {
                    playerPickupDrop.DropObject();
                }
            }
        }

        // Determine ingredient group and validate placement
        IngredientGroup ingredientGroup = GetIngredientGroup(other.tag);
        if (ingredientGroup == IngredientGroup.None) return;

        if (!ValidateIngredientGroup(ingredientGroup))
        {
            Debug.Log("Cannot mix ingredient groups!");
            return;
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
        else if (other.CompareTag("HeadSlice1") || other.CompareTag("HeadSlice2"))
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

    private IngredientGroup GetIngredientGroup(string tag)
    {
        switch (tag)
        {
            case "SushiRoll":
            case "SoySauceBowl":
                return IngredientGroup.Sushi;
            
            case "Balut":
            case "VinegarBowl":
                return IngredientGroup.Balut;
            
            case "HeadSlice1":
            case "HeadSlice2":
                return IngredientGroup.Goat;
            
            default:
                return IngredientGroup.None;
        }
    }

    private bool ValidateIngredientGroup(IngredientGroup newGroup)
    {
        // If no group is active yet, set the active group
        if (activeGroup == IngredientGroup.None)
        {
            activeGroup = newGroup;
            return true;
        }

        // Reject if trying to add to a different group
        return activeGroup == newGroup;
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
        if (!headSlot1Filled && head.CompareTag("HeadSlice1"))
        {
            PlaceIngredient(head, headSlot1, Quaternion.identity);
            headSlot1Filled = true;
        }
        else if (!headSlot2Filled && head.CompareTag("HeadSlice2"))
        {
            PlaceIngredient(head, headSlot2, Quaternion.identity);
            headSlot2Filled = true;
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
        ClearSlot(headSlot1);
        ClearSlot(headSlot2);
        headSlot1Filled = false;
        headSlot2Filled = false;

        // Reset completion flags and active group
        SushiComplete = false;
        BalutComplete = false;
        GoatComplete = false;
        activeGroup = IngredientGroup.None;
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

        // Check if both head slots are filled
        if (headSlot1Filled && headSlot2Filled)
        {
            GoatComplete = true;
            Debug.Log("Goat Complete!");
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

    public bool IsGoatComplete()
    {
        return GoatComplete;
    }
}