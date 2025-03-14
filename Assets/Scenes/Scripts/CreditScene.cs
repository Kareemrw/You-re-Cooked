using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScene : MonoBehaviour
{
    // Start is called before the first frame update
    void OnAwake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        ReturnToTitleScreen();
    }

    // Update is called once per frame
    IEnumerator ReturnToTitleScreen()
    {
        yield return new WaitForSeconds(0.20f);
        Debug.Log("emter return");

        Load3DScene();
    }

    public void Load3DScene()
    {
            SceneManager.LoadScene("TitleScene");
    }
}
