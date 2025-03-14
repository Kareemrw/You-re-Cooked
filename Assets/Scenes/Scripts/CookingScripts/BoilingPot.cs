using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilingPot : MonoBehaviour
{
    [Header("Cooking Settings")]
    [SerializeField] private float boilTime = 5f;
    [SerializeField] private float riceCookTime = 5f;
    [SerializeField] private float alfinCookTime = 5f; // Cooking time for Alfin halves

    [Header("Prefabs")]
    [SerializeField] private GameObject boiledEggPrefab;
    [SerializeField] private GameObject cookedAlfinHalf1Prefab; // Cooked prefab for AlfinHalf1
    [SerializeField] private GameObject cookedAlfinHalf2Prefab; // Cooked prefab for AlfinHalf2

    [Header("Visuals")]
    [SerializeField] private GameObject steam;
    [SerializeField] private GameObject riceObject;
    [SerializeField] private GameObject waterObject;

    [Header("Interaction Settings")]
    [SerializeField] private int requiredHits = 100;

    // Trackers
    private int waterHits = 0;
    private int riceHits = 0;

    private bool riceActivated;
    private bool waterActivated;

    public bool isBoiling = false;
    private bool isCookingRice = false;
    private bool isCookingAlfinHalf1 = false;
    private bool isCookingAlfinHalf2 = false;

    private float timer = 0f;
    private float riceCookTimer = 0f;
    private float alfinCookTimer = 0f;

    private GameObject eggToBoil;
    private GameObject alfinHalf1ToCook;
    private GameObject alfinHalf2ToCook;

    private HashSet<GameObject> eggsInside = new HashSet<GameObject>();
    private HashSet<GameObject> alfinHalf1Inside = new HashSet<GameObject>();
    private HashSet<GameObject> alfinHalf2Inside = new HashSet<GameObject>();

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
        else if (other.CompareTag("AlfinHalf1"))
        {
            alfinHalf1Inside.Add(other.gameObject);

            if (!isCookingAlfinHalf1 && steam != null && steam.activeSelf)
            {
                StartCookingAlfinHalf1(other.gameObject);
            }
        }
        else if (other.CompareTag("AlfinHalf2"))
        {
            alfinHalf2Inside.Add(other.gameObject);

            if (!isCookingAlfinHalf2 && steam != null && steam.activeSelf)
            {
                StartCookingAlfinHalf2(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Egg"))
        {
            eggsInside.Remove(other.gameObject);
        }
        else if (other.CompareTag("AlfinHalf1"))
        {
            alfinHalf1Inside.Remove(other.gameObject);
        }
        else if (other.CompareTag("AlfinHalf2"))
        {
            alfinHalf2Inside.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        // Egg boiling process
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

        // Rice cooking process
        if (!isCookingRice && waterActivated && riceActivated && steam != null && steam.activeSelf)
        {
            isCookingRice = true;
            riceCookTimer = 0f;
            Debug.Log("Started cooking rice.");
        }

        if (isCookingRice)
        {
            riceCookTimer += Time.deltaTime;
            if (riceCookTimer >= riceCookTime)
            {
                if (riceObject != null)
                {
                    riceObject.tag = "CookedRice";
                    Debug.Log("Rice has finished cooking and is now tagged as CookedRice.");
                }
                else
                {
                    Debug.Log("Rice cooked, but riceObject is not set.");
                }

                if (waterObject != null)
                {
                    waterObject.SetActive(false);
                }
                if (steam != null)
                {
                    steam.SetActive(false);
                }

                waterActivated = false;
                riceActivated = false;
                isCookingRice = false;
                riceCookTimer = 0f;
                Debug.Log("Finished cooking rice. Water and steam deactivated.");
            }
        }

        // AlfinHalf1 cooking process
        if (!isCookingAlfinHalf1 && steam != null && steam.activeSelf)
        {
            foreach (var alfinHalf1 in alfinHalf1Inside)
            {
                StartCookingAlfinHalf1(alfinHalf1);
                break;
            }
        }

        if (isCookingAlfinHalf1 && alfinHalf1ToCook != null)
        {
            alfinCookTimer += Time.deltaTime;
            if (alfinCookTimer >= alfinCookTime)
            {
                Vector3 alfinPosition = alfinHalf1ToCook.transform.position;
                Quaternion alfinRotation = alfinHalf1ToCook.transform.rotation;

                alfinHalf1Inside.Remove(alfinHalf1ToCook);
                Destroy(alfinHalf1ToCook);
                Instantiate(cookedAlfinHalf1Prefab, alfinPosition, alfinRotation);

                isCookingAlfinHalf1 = false;
                alfinCookTimer = 0f;
            }
        }

        // AlfinHalf2 cooking process
        if (!isCookingAlfinHalf2 && steam != null && steam.activeSelf)
        {
            foreach (var alfinHalf2 in alfinHalf2Inside)
            {
                StartCookingAlfinHalf2(alfinHalf2);
                break;
            }
        }

        if (isCookingAlfinHalf2 && alfinHalf2ToCook != null)
        {
            alfinCookTimer += Time.deltaTime;
            if (alfinCookTimer >= alfinCookTime)
            {
                Vector3 alfinPosition = alfinHalf2ToCook.transform.position;
                Quaternion alfinRotation = alfinHalf2ToCook.transform.rotation;

                alfinHalf2Inside.Remove(alfinHalf2ToCook);
                Destroy(alfinHalf2ToCook);
                Instantiate(cookedAlfinHalf2Prefab, alfinPosition, alfinRotation);

                isCookingAlfinHalf2 = false;
                alfinCookTimer = 0f;
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Rice") && !riceActivated)
        {
            riceHits++;
            if (riceHits >= requiredHits)
            {
                ActivateRice();
            }
        }
        if (other.CompareTag("Water") && !waterActivated)
        {
            waterHits++;
            if (waterHits >= requiredHits)
            {
                ActivateWater();
            }
        }
    }

    private void ActivateRice()
    {
        riceActivated = true;
        if (riceObject != null)
        {
            riceObject.SetActive(true);
        }
        Debug.Log("Rice activated.");
    }

    private void ActivateWater()
    {
        waterActivated = true;
        if (waterObject != null)
        {
            waterObject.SetActive(true);
        }
        Debug.Log("Water activated.");
    }

    private void StartBoiling(GameObject egg)
    {
        if (!isBoiling)
        {
            isBoiling = true;
            eggToBoil = egg;
        }
    }

    private void StartCookingAlfinHalf1(GameObject alfinHalf1)
    {
        if (!isCookingAlfinHalf1)
        {
            isCookingAlfinHalf1 = true;
            alfinHalf1ToCook = alfinHalf1;
        }
    }

    private void StartCookingAlfinHalf2(GameObject alfinHalf2)
    {
        if (!isCookingAlfinHalf2)
        {
            isCookingAlfinHalf2 = true;
            alfinHalf2ToCook = alfinHalf2;
        }
    }
}