using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiRoller : MonoBehaviour
{
    [SerializeField] private Transform[] ingredientSlots;
    private bool[] slotFilled;
    [SerializeField] private float rollingVelocityThreshold = 2f;
    [SerializeField] private GameObject sushiPrefab;

    private Rigidbody rollerRigidbody;
    private bool isRolling = false;
    private int currentStep = 0; 
    private int salmonCounter = 0;
    private void Start()
    {
        rollerRigidbody = GetComponent<Rigidbody>();
        slotFilled = new bool[ingredientSlots.Length];
    }

    private void OnTriggerEnter(Collider other)
    {
        ObjectGrabbable foodItem = other.GetComponent<ObjectGrabbable>();
        if (foodItem != null && !foodItem.isHeld)
        {
            if (currentStep == 0 && other.CompareTag("Seaweed") && !slotFilled[0])
            {
                PlaceIngredient(other.transform, 0);
                currentStep++;
            }
            else if (currentStep == 1 && other.CompareTag("Rice") && !slotFilled[1])
            {
                PlaceIngredient(other.transform, 1);
                currentStep++;
            }
            else if (currentStep == 2 && other.CompareTag("Salmon"))
            {
                if (other.GetComponent<ObjectGrabbable>().isHeld) return;
                for (int slotIndex = 2; slotIndex <= 5; slotIndex++)
                {
                    if (!slotFilled[slotIndex])
                    {
                        slotFilled[slotIndex] = true;
                        
                        PlaceIngredient(other.transform, slotIndex);
                        salmonCounter++;

                        Debug.Log($"salmon in slot {slotIndex} total: {salmonCounter}/4");
                        other.enabled = false;

                        if (salmonCounter >= 4)
                        {
                            currentStep++;
                            Debug.Log("all salmon placed.");
                        }
                        break;
                    }
                }
            }
            else if (currentStep == 3 && other.CompareTag("CreamCheese") && !slotFilled[6])
            {
                PlaceIngredient(other.transform, 6);
                currentStep++;
            }
            else if (currentStep == 4 && other.CompareTag("Avocado") && !slotFilled[7])
            {
                PlaceIngredient(other.transform, 7);
                currentStep++;
            }
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
        if (AllSlotsFilled() && !isRolling)
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
        Instantiate(sushiPrefab, sushiPosition, sushiRotation);

        ClearRoller();
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

        currentStep = 0;

        isRolling = false;
    }
}