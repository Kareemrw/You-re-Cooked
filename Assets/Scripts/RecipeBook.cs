using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBook : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject recipeBookUI;
    public GameObject[] pages; // Assign 3 pages in order (Page1, Page2, Page3)

    private int currentPage = 1;
    private bool bookOpen = false;

    private void Start()
    {
        // Initialize all pages as inactive
        recipeBookUI.SetActive(false);
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }
    }

    private void Update()
    {
        // Toggle book visibility
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleBook();
        }

        if (bookOpen)
        {
            // Page navigation
            if (Input.GetKeyDown(KeyCode.D))
            {
                NextPage();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                PreviousPage();
            }
        }
    }

    private void ToggleBook()
    {
        bookOpen = !bookOpen;
        recipeBookUI.SetActive(bookOpen);

        if (bookOpen)
        {
            currentPage = 1;
            UpdatePages();
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