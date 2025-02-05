using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPickupDrop : MonoBehaviour
{
    [SerializeField] private GameObject grabUI;
    [SerializeField] private GameObject dropUI;
    [SerializeField] private GameObject throwUI;
    [SerializeField] private GameObject RotateUI;
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private Slider throwPowerBar;
    [SerializeField] private float maxThrowForce = 30f;
    [SerializeField] private float chargeTime = 2f;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] public float pickupDistance = 5f;

    private ObjectGrabbable objectGrabbable;
    private bool isChargingThrow = false;
    private bool isRotatingObject = false;
    private float chargeTimer = 0f;

    private void Start()
    {
        throwPowerBar.gameObject.SetActive(false);
        dropUI.gameObject.SetActive(false);
        throwUI.gameObject.SetActive(false);
        RotateUI.gameObject.SetActive(false);
    }
    void Update()
    {
        PickupDrop();
        ThrowCharge();
        ObjectRotation();
    }

    private void PickupDrop()
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
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit hit, pickupDistance, pickupLayerMask))
        {
            if (hit.transform.TryGetComponent(out objectGrabbable))
            {
                Debug.Log("Hit Layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));
                grabUI.gameObject.SetActive(false);
                dropUI.gameObject.SetActive(true);
                throwUI.gameObject.SetActive(true);
                RotateUI.gameObject.SetActive(true);
                objectGrabbable.Grab(objectGrabPointTransform);
            }
        }
    }

     private void ObjectRotation()
    {
        if (Input.GetMouseButtonDown(1) && objectGrabbable != null)
        {
            isRotatingObject = !isRotatingObject;

            if (isRotatingObject)
            {
                mouseLook.enabled = false;
                objectGrabbable.ToggleRotationLock(false);
            }
            else
            {
                mouseLook.enabled = true;
                objectGrabbable.ToggleRotationLock(true);
            }
        }
        if (isRotatingObject)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            objectGrabbable.transform.Rotate(
                playerCameraTransform.up,
                -mouseX,
                Space.World
            );
            objectGrabbable.transform.Rotate(
                playerCameraTransform.right,
                mouseY,
                Space.World
            );
        }
    }

    private void DropObject()
    {
        if (isRotatingObject)
        {
            isRotatingObject = false;
            mouseLook.enabled = true;
            objectGrabbable.ToggleRotationLock(true);
        }

        objectGrabbable.Drop();
        grabUI.gameObject.SetActive(true);
        dropUI.gameObject.SetActive(false);
        throwUI.gameObject.SetActive(false);
        RotateUI.gameObject.SetActive(false);
        objectGrabbable = null;
    }

    private void ThrowObject()
    {
        if (isRotatingObject)
        {
            isRotatingObject = false;
            mouseLook.enabled = true;
            objectGrabbable.ToggleRotationLock(true);
        }

        float actualCharge = (chargeTimer % chargeTime) / chargeTime;
        float throwForce = Mathf.Lerp(5f, maxThrowForce, actualCharge);
        objectGrabbable.Throw(playerCameraTransform.forward, throwForce);
        grabUI.gameObject.SetActive(true);
        dropUI.gameObject.SetActive(false);
        throwUI.gameObject.SetActive(false);
        RotateUI.gameObject.SetActive(false);
        objectGrabbable = null;
    }
    private void ThrowCharge()
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
    private void ResetCharge()
    {
        isChargingThrow = false;
        chargeTimer = 0f;
        throwPowerBar.value = 0f;
        throwPowerBar.gameObject.SetActive(false);
    }
}
