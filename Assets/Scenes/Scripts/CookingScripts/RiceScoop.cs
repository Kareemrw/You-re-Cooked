using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiceScoop : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        // Check if the collided object is a paddle
        if (collision.gameObject.CompareTag("Paddle")&&gameObject.CompareTag("CookedRice"))
        {
            // Reset the rice's tag to default (assumed "Rice")
            gameObject.tag = "Untagged";

            // Deactivate this rice GameObject
            gameObject.SetActive(false);

            // Activate the child on the paddle called "riceChild"
            Transform riceChild = collision.transform.Find("RiceChild");
            if (riceChild != null)
            {
                riceChild.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("The paddle does not have a child named 'riceChild'.");
            }
        }
    }
}
