using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilingPot : MonoBehaviour
{
    [SerializeField] private float boilTime = 5f;
    [SerializeField] private float riceCookTime = 5f;
    [SerializeField] private GameObject boiledEggPrefab;
    [SerializeField] private GameObject steam;
    [SerializeField] private GameObject riceObject; 
    [SerializeField] private GameObject waterObject; 
    [SerializeField] private int requiredHits = 100;
    
    private int waterHits = 0;
    private int riceHits = 0;
    
    private bool riceActivated;
    private bool waterActivated;
    
    public bool isBoiling = false;
    private bool isCookingRice = false;
    
    private float timer = 0f;
    private float riceCookTimer = 0f;
    
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
        
        // Rice cooking process: Start when water and rice are activated and the steam is on
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
                // Rice has finished cooking: change its tag to "CookedRice"
                if (riceObject != null)
                {
                    riceObject.tag = "CookedRice";
                    Debug.Log("Rice has finished cooking and is now tagged as CookedRice.");
                }
                else
                {
                    Debug.Log("Rice cooked, but riceObject is not set.");
                }
                
                // Deactivate water and steam once rice is cooked.
                if (waterObject != null)
                {
                    waterObject.SetActive(false);
                }
                if (steam != null)
                {
                    steam.SetActive(false);
                }
                
                // Reset the rice cooking flags so it can be triggered again.
                waterActivated = false;
                riceActivated = false;
                isCookingRice = false;
                riceCookTimer = 0f;
                Debug.Log("Finished cooking rice. Water and steam deactivated.");
            }
        }
    }
    
    // Separate hit counters for water and rice particles
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
}
