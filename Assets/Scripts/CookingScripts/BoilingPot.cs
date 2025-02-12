using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilingPot : MonoBehaviour
{
    [SerializeField] private float boilTime = 5f;
    [SerializeField] private GameObject boiledEggPrefab;
    [SerializeField] private GameObject steam;

    public bool isBoiling = false;
    private float timer = 0f;
    private GameObject eggToBoil;
    private HashSet<GameObject> eggsInside = new HashSet<GameObject>();

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
            eggsInside.Add(other.gameObject);
            
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
            eggsInside.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        if (!isBoiling && steam != null && steam.activeSelf)
        {
            foreach (var egg in eggsInside)
            {
                StartBoiling(egg);
                break;
            }
        }

        if (isBoiling && eggToBoil != null)
        {
            timer += Time.deltaTime;
            if (timer >= boilTime)
            {
                Vector3 eggPosition = eggToBoil.transform.position;
                Quaternion eggRotation = eggToBoil.transform.rotation;

                eggsInside.Remove(eggToBoil);
                Destroy(eggToBoil);
                Instantiate(boiledEggPrefab, eggPosition, eggRotation);

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
