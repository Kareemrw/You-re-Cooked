using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Transform[] foodSlots;
    private int currentSlot = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food") && currentSlot < foodSlots.Length)
        {
            PlaceFoodOnPlate(other.transform);
        }
    }

    private void PlaceFoodOnPlate(Transform food)
    {
        food.position = foodSlots[currentSlot].position;
        food.rotation = foodSlots[currentSlot].rotation;
        Collider foodCollider = food.GetComponent<Collider>();
        if (foodCollider != null)
        {
            foodCollider.enabled = false;
        }
        Rigidbody foodRigidbody = food.GetComponent<Rigidbody>();
        if (foodRigidbody != null)
        {
            Destroy(foodRigidbody); 
        }
        food.SetParent(foodSlots[currentSlot]);

        currentSlot++;
    }

}