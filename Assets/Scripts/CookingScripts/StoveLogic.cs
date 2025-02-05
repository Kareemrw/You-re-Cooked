using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamActivator : MonoBehaviour
{
    [SerializeField] private float steamDelay = 10f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pot"))
        {
            Transform steamChild = other.transform.Find("Steam");
            Transform liquidChild = other.transform.Find("Liquid");

            if (steamChild != null && liquidChild != null && liquidChild.gameObject.activeSelf)
            {
                StartCoroutine(ActivateSteamWithDelay(steamChild.gameObject));
            }
            else
            {
                Debug.LogWarning("Liquid child not found or inactive on the pot GameObject.");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pot"))
        {
            Transform steamChild = other.transform.Find("Steam");
            Transform liquidChild = other.transform.Find("Liquid");

            if (steamChild != null && liquidChild != null && liquidChild.gameObject.activeSelf)
            {
                StartCoroutine(DeActivateSteamWithDelay(steamChild.gameObject));
            }
            else
            {
                Debug.LogWarning("Liquid child not found or inactive on the pot GameObject.");
            }
        }
    }

    private IEnumerator ActivateSteamWithDelay(GameObject steamChild)
    {
        yield return new WaitForSeconds(steamDelay); 
        steamChild.SetActive(true); 
    }
    private IEnumerator DeActivateSteamWithDelay(GameObject steamChild)
    {
        yield return new WaitForSeconds(steamDelay);
        steamChild.SetActive(false);
    }
}