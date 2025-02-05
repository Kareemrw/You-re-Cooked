using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Transform[] foodSlots;
    private int currentSlot = 0;

    private void OnTriggerEnter(Collider other)
    {
        ObjectGrabbable foodItem = other.GetComponent<ObjectGrabbable>();
        if (other.CompareTag("Food") && !foodItem.isHeld && currentSlot < foodSlots.Length)
        {
            PlaceFoodOnPlate(other.transform);
        }
    }

    private void PlaceFoodOnPlate(Transform food)
    {
        // Snap food to the next available slot
        food.position = foodSlots[currentSlot].position;
        food.rotation = foodSlots[currentSlot].rotation;

        Collider foodCollider = food.GetComponent<Collider>();
        if (foodCollider != null)
        {
            foodCollider.enabled = false;
        }

        // Disable physics on the food
        Rigidbody foodRigidbody = food.GetComponent<Rigidbody>();
        if (foodRigidbody != null)
        {
            foodRigidbody.isKinematic = true;
        }

        food.SetParent(foodSlots[currentSlot]);

        currentSlot++;
    }
}