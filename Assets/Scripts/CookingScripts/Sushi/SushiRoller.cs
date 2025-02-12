using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiRoller : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] private Transform[] ingredientSlots;
    private bool[] slotFilled;

    [Header("Rolling Settings")]
    [SerializeField] private float rollingVelocityThreshold = 2f;
    [SerializeField] private GameObject sushiPrefab;

    private Rigidbody rollerRigidbody;
    private bool isRolling = false;
    private int currentStep = 0; 

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
            else if (currentStep == 2 && other.CompareTag("Salmon") && !slotFilled[2])
            {
                PlaceIngredient(other.transform, 2);
                currentStep++;
            }
            else if (currentStep == 3 && other.CompareTag("CreamCheese") && !slotFilled[3])
            {
                PlaceIngredient(other.transform, 3);
                currentStep++;
            }
            else if (currentStep == 4 && other.CompareTag("Avocado") && !slotFilled[4])
            {
                PlaceIngredient(other.transform, 4);
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

        Debug.Log("🎉 Sushi rolled and ready to serve!");
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