using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : MonoBehaviour
{
    [SerializeField] private Transform ingredientSlot;
    private GameObject currentIngredient;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WholeIngredient") && currentIngredient == null)
        {
            ObjectGrabbable grabbable = other.GetComponent<ObjectGrabbable>();
            if (grabbable != null && !grabbable.isHeld)
            {
                PlaceIngredient(other.gameObject);
            }
        }
        if (other.CompareTag("SushiRoll") && currentIngredient == null)
        {
            ObjectGrabbable grabbable = other.GetComponent<ObjectGrabbable>();
            if (grabbable != null && !grabbable.isHeld)
            {
                PlaceIngredient(other.gameObject);
            }
        }
        if (other.CompareTag("AlfinHead") && currentIngredient == null)
        {
            ObjectGrabbable grabbable = other.GetComponent<ObjectGrabbable>();
            if (grabbable != null && !grabbable.isHeld)
            {
                PlaceIngredient(other.gameObject);
            }
        }
    }

    private void PlaceIngredient(GameObject ingredient)
    {
         Renderer renderer = ingredient.GetComponent<Renderer>();
    if (renderer == null) return;

    Vector3 ingredientSize = renderer.bounds.size;
        ingredient.transform.position = ingredientSlot.position + new Vector3(0, ingredientSize.y / 2, 0);
         if (ingredient.CompareTag("SushiRoll")) 
        {  
            ingredient.transform.rotation = ingredientSlot.rotation; 
        } 
        else if (ingredient.CompareTag("AlfinHead")) 
        {
            ingredient.transform.rotation = ingredientSlot.rotation * Quaternion.Euler(0, 90, 90); 
            
            ingredient.transform.position = ingredientSlot.position + new Vector3(0, 0.2f, 0);
        }   
        else
        {
            ingredient.transform.position = ingredientSlot.position;
            ingredient.transform.rotation = ingredientSlot.rotation;
        }
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; 
        }

        ingredient.transform.SetParent(ingredientSlot);
        currentIngredient = ingredient;
    }

    private void Update()
    {
        if (currentIngredient == null) return;
        ObjectGrabbable grabbable = currentIngredient.GetComponent<ObjectGrabbable>();
        bool isChopped = currentIngredient.GetComponent<KnifeChop>()?.isChopped ?? false;

        if ((grabbable != null && grabbable.isHeld) || isChopped)
        {
            ClearCuttingBoard();
        }
    }

    private void ClearCuttingBoard()
    {
        if (currentIngredient != null)
        {
            Rigidbody rb = currentIngredient.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            currentIngredient.transform.SetParent(null);
            currentIngredient = null;
        }
    }
}