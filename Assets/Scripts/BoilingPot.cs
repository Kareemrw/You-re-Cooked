using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilingPot : MonoBehaviour
{
    [SerializeField] private float boilTime = 5f; // Time to boil eggs
    [SerializeField] private GameObject boiledEggPrefab; // Prefab for boiled eggs
    [SerializeField] private GameObject steam; // Reference to the Steam GameObject

    private bool isBoiling = false;
    private float timer = 0f;
    private GameObject eggToBoil;
    private HashSet<GameObject> eggsInside = new HashSet<GameObject>(); // Track eggs inside the pot

    private void Start()
    {
        if (steam == null)
        {
            steam = transform.Find("Steam")?.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Egg"))
        {
            eggsInside.Add(other.gameObject); // Store egg in the pot
            
            // If steam is active and no egg is currently boiling, start boiling
            if (!isBoiling && steam != null && steam.activeSelf)
            {
                StartBoiling(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Egg"))
        {
            eggsInside.Remove(other.gameObject); // Remove egg from the pot tracking
        }
    }

    private void Update()
    {
        // Check if steam just turned on while eggs are inside
        if (!isBoiling && steam != null && steam.activeSelf)
        {
            foreach (var egg in eggsInside)
            {
                StartBoiling(egg);
                break; // Start boiling only one egg at a time
            }
        }

        if (isBoiling && eggToBoil != null)
        {
            timer += Time.deltaTime;
            if (timer >= boilTime)
            {
                Vector3 eggPosition = eggToBoil.transform.position;
                Quaternion eggRotation = eggToBoil.transform.rotation;

                eggsInside.Remove(eggToBoil); // Remove from tracking
                Destroy(eggToBoil); // Remove raw egg
                Instantiate(boiledEggPrefab, eggPosition, eggRotation); // Spawn boiled egg

                isBoiling = false;
                timer = 0f;
            }
        }
    }

    private void StartBoiling(GameObject egg)
    {
        if (!isBoiling)
        {
            isBoiling = true;
            eggToBoil = egg;
        }
    }
}
