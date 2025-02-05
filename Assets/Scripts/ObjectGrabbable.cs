using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;
    private bool isRotationLocked = true;
    
    public bool isHeld = true;
    

    private void Awake()
    {
        objectRigidBody = GetComponent<Rigidbody>();
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidBody.useGravity = false;
        objectRigidBody.drag = 5f;
        objectRigidBody.isKinematic = true;
        ToggleRotationLock(true);

        isHeld = true;
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidBody.useGravity = true;
        objectRigidBody.drag = 0.5f;
        objectRigidBody.isKinematic = false;
        objectRigidBody.freezeRotation = false;

        isHeld = false;
    }

    public void Throw(UnityEngine.Vector3 throwDirection, float throwForce)
    {
        objectRigidBody.drag = 0.5f;
        objectRigidBody.useGravity = true;
        objectRigidBody.isKinematic = false;
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
        isRotationLocked = isLocked;
        objectRigidBody.freezeRotation = isLocked;
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 15f;
            UnityEngine.Vector3 newPosition = UnityEngine.Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidBody.MovePosition(newPosition);
        }
    }
}
