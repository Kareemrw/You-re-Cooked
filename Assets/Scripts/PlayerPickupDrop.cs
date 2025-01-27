using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPickupDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private Slider throwPowerBar;
    [SerializeField] private float maxThrowForce = 30f;
    [SerializeField] private float chargeTime = 2f;

    private ObjectGrabbable objectGrabbable;
    private bool isChargingThrow = false;
    private float chargeTimer = 0f;

    private void Start()
    {
        throwPowerBar.gameObject.SetActive(false);
    }
    void Update()
    {
        HandlePickupDrop();
        HandleThrowCharge();
    }

    private void HandlePickupDrop()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null)
            {
                TryPickup();
            }
            else
            {
                DropObject();
            }
        }
    }

    private void TryPickup()
    {
        float pickupDistance = 2f;
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit hit, pickupDistance, pickupLayerMask))
        {
            if (hit.transform.TryGetComponent(out objectGrabbable))
            {
                objectGrabbable.Grab(objectGrabPointTransform);
            }
        }
    }

    private void DropObject()
    {
        objectGrabbable.Drop();
        objectGrabbable = null;
    }

    private void HandleThrowCharge()
    {
        if (Input.GetMouseButtonDown(0) && objectGrabbable != null)
        {
            isChargingThrow = true;
            throwPowerBar.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonUp(0) && isChargingThrow)
        {
            ThrowObject();
            ResetCharge();
        }

        if (isChargingThrow)
        {
            chargeTimer += Time.deltaTime;
            float visualCharge = (chargeTimer % chargeTime) / chargeTime;
            throwPowerBar.value = visualCharge;
        }
    }

    private void ThrowObject()
    {
        float actualCharge = (chargeTimer % chargeTime) / chargeTime;
        float throwForce = Mathf.Lerp(5f, maxThrowForce, actualCharge);

        objectGrabbable.Throw(playerCameraTransform.forward, throwForce);
        objectGrabbable = null;
    }

    private void ResetCharge()
    {
        isChargingThrow = false;
        chargeTimer = 0f;
        throwPowerBar.value = 0f;
        throwPowerBar.gameObject.SetActive(false);
    }
}
