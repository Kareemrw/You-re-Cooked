using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;
    public bool isHeld = false;



    private void Awake()
    {
        objectRigidBody = GetComponent<Rigidbody>();

    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidBody.useGravity = false;
        objectRigidBody.drag = 5f;

        ToggleRotationLock(true);
        isHeld = true;
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidBody.useGravity = true;
        objectRigidBody.drag = 0.5f;

        ToggleRotationLock(false); 
        isHeld = false;
    }

    public void Throw(UnityEngine.Vector3 throwDirection, float throwForce)
    {
        objectRigidBody.drag = 0.5f;
        objectRigidBody.useGravity = true;
        objectRigidBody.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        objectGrabPointTransform = null;
        ToggleRotationLock(false);

        EggCrack egg = GetComponent<EggCrack>();
        if (egg != null)
        {
            egg.MarkAsThrown();
        }

        isHeld = false;
    }

    public void ToggleRotationLock(bool isLocked)
    {
        objectRigidBody.freezeRotation = isLocked;
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 15f;
            UnityEngine.Vector3 targetPosition = UnityEngine.Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.fixedDeltaTime * lerpSpeed);
            UnityEngine.Vector3 moveDirection = targetPosition - objectRigidBody.position;
            objectRigidBody.velocity = moveDirection / Time.fixedDeltaTime;
        }
    }
}