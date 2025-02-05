using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] private PlayerPickupDrop playerPickupDrop; 
    [SerializeField] private Transform playerCameraTransform; 
    [SerializeField] private LayerMask grabLayerMask; 
    private Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
        }

        outline.enabled = false; 
    }

    private void Update()
    {
        if (playerPickupDrop == null || playerCameraTransform == null) return;

        float grabDistance = playerPickupDrop.pickupDistance;

        Ray ray = new Ray(playerCameraTransform.position, playerCameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, grabLayerMask))
        {
            ObjectGrabbable grabbable = hit.collider.GetComponent<ObjectGrabbable>();
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