using UnityEngine;

public class RecipeBook : MonoBehaviour
{
    public GameObject recipeBookUI;
    public GameObject[] pages;
    public PlayerMovement playerMovement; // Reference to player movement script
    public MouseLook mouseLook;
    private int currentPage = 1;
    private bool bookOpen = false;

    private void Start()
    {
        recipeBookUI.SetActive(false);
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }
        
        // Lock cursor initially
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleBook();
        }

        if (bookOpen)
        {
            HandlePageNavigation();
        }
    }

    private void ToggleBook()
    {
        bookOpen = !bookOpen;
        recipeBookUI.SetActive(bookOpen);

        // Toggle player movement
        if (playerMovement != null && mouseLook != null)
        {
            playerMovement.enabled = !bookOpen;
            mouseLook.enabled = !bookOpen;
        }
        
        // Handle cursor state
        Cursor.lockState = bookOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = bookOpen;

        if (bookOpen)
        {
            currentPage = 1;
            UpdatePages();
        }
    }

    private void HandlePageNavigation()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            NextPage();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            PreviousPage();
        }
    }

    private void NextPage()
    {
        if (currentPage < pages.Length)
        {
            currentPage++;
            UpdatePages();
        }
    }

    private void PreviousPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
            UpdatePages();
        }
    }

    private void UpdatePages()
    {
        // Deactivate all pages
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }

        // Activate current page
        if (currentPage >= 1 && currentPage <= pages.Length)
        {
            pages[currentPage - 1].SetActive(true);
        }
    }
}