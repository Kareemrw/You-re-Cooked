using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkLogic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pot"))
        {
            // Find the liquid child of the pot
            Transform liquidChild = other.transform.Find("Liquid");
            
            if (liquidChild != null)
            {
                liquidChild.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Liquid child not found on the pot GameObject.");
            }
        }
    }
}