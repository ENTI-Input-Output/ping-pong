using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class VRAnimatorController : MonoBehaviour
{
    private Animator animator;
    private Vector3 previousPos;
    private VRRig vrRig;

    public float speedTreshold = 0.1f;
    [Range(0, 1)]
    public float smoothingTransition = 1;


    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
        vrRig = GetComponent<VRRig>();
        previousPos = vrRig.head.vrTarget.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Compute the speed
        Vector3 headsetSpeed = (vrRig.head.vrTarget.position - previousPos) / Time.deltaTime;
        headsetSpeed.y = 0;
        //Local Speed
        Vector3 headsetLocalSpeed = transform.InverseTransformDirection(headsetSpeed);
        previousPos = vrRig.head.vrTarget.position;

        //Set Animator values
        float previousDirectionX = animator.GetFloat("directionX");
        float previousDirectionY = animator.GetFloat("directionY");

        animator.SetBool("isMoving", headsetLocalSpeed.magnitude > speedTreshold);
        animator.SetFloat("directionX", Mathf.Lerp(previousDirectionX, Mathf.Clamp(headsetLocalSpeed.x, -1, 1), smoothingTransition));
        animator.SetFloat("directionY", Mathf.Lerp(previousDirectionY, Mathf.Clamp(headsetLocalSpeed.z, -1, 1), smoothingTransition));
    } 
}
