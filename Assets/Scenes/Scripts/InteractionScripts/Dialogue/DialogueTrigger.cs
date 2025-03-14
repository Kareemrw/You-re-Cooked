using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    public GameObject plate;
    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if(playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if(InputManager.GetInstance().GetInteractPressed())
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Plate")
        {
            playerInRange = true;

            if(plate.GetComponent<Plate>().BalutComplete == true || plate.GetComponent<Plate>().GoatComplete == true || plate.GetComponent<Plate>().SushiComplete == true)
            {
                Load2DScene();
            }

            
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    public void Load2DScene()
    {
        if (plate.GetComponent<Plate>().BalutComplete == true)
        {
            SceneManager.LoadScene("BalutWin");
        }
        if (plate.GetComponent<Plate>().GoatComplete == true)
        {
            SceneManager.LoadScene("GHWin");
        }
        if (plate.GetComponent<Plate>().SushiComplete == true)
        {
            SceneManager.LoadScene("SushiWin");
        }

    }
}
