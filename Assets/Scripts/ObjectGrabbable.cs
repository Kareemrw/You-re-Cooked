using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;

    private void Awake()
    {
        objectRigidBody = GetComponent<Rigidbody>();
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidBody.useGravity = false;
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidBody.useGravity = true;
    }

    public void Throw(UnityEngine.Vector3 throwDirection, float throwForce)
    {
        objectRigidBody.useGravity = true;
        objectRigidBody.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        objectGrabPointTransform = null;
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            UnityEngine.Vector3 newPosition = UnityEngine.Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidBody.MovePosition(newPosition);
        }
    }
}
