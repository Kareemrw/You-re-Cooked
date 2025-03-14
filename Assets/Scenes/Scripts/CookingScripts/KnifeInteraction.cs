using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeInteraction : MonoBehaviour
{
    [Header("Knife Settings")]
    [SerializeField] private GameObject creamCheeseChild; // Reference to the cream cheese child
    [SerializeField] private GameObject avocadoChild;    // Reference to the avocado child

    private void OnTriggerEnter(Collider other)
    {
        // Check if the knife collides with a container
        if (other.CompareTag("CreamCheeseContainer"))
        {
            ActivateChild(creamCheeseChild);
        }
        else if (other.CompareTag("AvocadoContainer"))
        {
            ActivateChild(avocadoChild);
        }
    }

    private void ActivateChild(GameObject child)
    {
        if (child != null)
        {
            child.SetActive(true);
            Debug.Log($"{child.name} activated!");
        }
        else
        {
            Debug.LogWarning("Child reference is missing!");
        }
    }
}