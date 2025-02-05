using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] private PlayerPickupDrop playerPickupDrop; // Reference to PlayerPickupDrop
    [SerializeField] private Transform playerCameraTransform; // Player's camera
    [SerializeField] private LayerMask grabLayerMask; // Layer for grabbable objects
    private Outline outline;

    private void Start()
    {
        // Ensure Outline component is assigned
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>(); // Add Outline if missing
        }

        outline.enabled = false; // Disable outline by default
    }

    private void Update()
    {
        if (playerPickupDrop == null || playerCameraTransform == null) return;

        float grabDistance = playerPickupDrop.pickupDistance;

        // Raycast from camera to check for grabbable objects
        Ray ray = new Ray(playerCameraTransform.position, playerCameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, grabLayerMask))
        {
            ObjectGrabbable grabbable = hit.collider.GetComponent<ObjectGrabbable>();

            // Enable outline only if the object is not being held
            if (grabbable != null)
            {
                if(grabbable.isHeld)
                {
                    return;
                }
                outline.enabled = hit.collider.gameObject == gameObject;
            }
            else
            {
                outline.enabled = false;
            }
        }
        else
        {
            outline.enabled = false;
        }
    }
}