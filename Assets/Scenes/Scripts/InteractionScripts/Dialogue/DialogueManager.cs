using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
   
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;


    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }

    private static DialogueManager instance;

    // public GameObject instructionsPanel;

    // public GameObject backgroundArt;

    //public GameObject frames;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the Scene");
        }
        instance = this;



        if(SceneManager.GetActiveScene().name == "IntroductionDialogue" || SceneManager.GetActiveScene().name == "BalutIntro" || SceneManager.GetActiveScene().name == "BalutWin" ||
                SceneManager.GetActiveScene().name == "GHIntro" || SceneManager.GetActiveScene().name == "GHWin" ||
                    SceneManager.GetActiveScene().name == "SushiIntro" || SceneManager.GetActiveScene().name == "SushiWin" || SceneManager.GetActiveScene().name == "QueenEnding")
        {
            //get all of the choices text
            choicesText = new TextMeshProUGUI[choices.Length];
            int index = 0;
            foreach (GameObject choice in choices)
            {
                choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
                index++;
            }

            EnterDialogueMode(inkJSON);
        }

        if(SceneManager.GetActiveScene().name == "BalutLoss" || SceneManager.GetActiveScene().name == "GHLoss" || SceneManager.GetActiveScene().name == "SushiLoss")
        {
            EnterDialogueMode(inkJSON);
        }
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    /*private void Start()
    {
        //dialogueIsPlaying = false;
        //dialoguePanel.SetActive(false);

        //instructionsPanel.SetActive(true);
        //backgroundArt.SetActive(false);

        //get all of the choices text
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }*/

    private void Update()
    {
        //return right away if dialogue isn't playing
        if(!dialogueIsPlaying)
        {
            return;
        }

        //handle continuing to the next line in the dialogue when submit is pressed
        if(InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueStory();
        }
    }



    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        //instructionsPanel.SetActive(false);
        //backgroundArt.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        //instructionsPanel.SetActive(true);
        //backgroundArt.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        

        Load3DScene();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //set text for the current dialogue line
            dialogueText.text = currentStory.Continue();
            //display choices, if any, for this dialogue line
            DisplayChoices();

            Debug.Log("cont story if statement");
        }
        else
        {
            //frames.SetActive(false);
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        //definsive check to make sure our UI can support the number of choices coming in
        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        // enable and initialize the choices up to amomunt of choices for this line of dialogue
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
            
            Debug.Log("foreach loop");
        }
        // go through the remaining choices the UI supports and make sure they are hidden
        for(int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
            Debug.Log("for loop");
        }

       SelectFirstChoice();
        //frames.SetActive(true);
    }

    private void SelectFirstChoice()
    {
        
        Debug.Log("select first block");
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();

        //frames.SetActive(false);
        Debug.Log("enters makeChoice block");
    }

    public void Load3DScene()
    {
        if(SceneManager.GetActiveScene().name == "BalutLoss" || SceneManager.GetActiveScene().name == "GHLoss" || SceneManager.GetActiveScene().name == "SushiLoss"  || SceneManager.GetActiveScene().name == "QueenEnding")
        {
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene("TestScene");
        }
        

    }
}
