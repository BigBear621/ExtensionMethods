using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMethods;


public class ExtensionMethodTest : MonoBehaviour
{
    public AudioClip[] clip;
    private AudioSource source0;
    private AudioSource source1;

    private Rigidbody body;
    private Vector3 towards;
    private float moveSpeed = 10;

    private float lookSpeed = 100f;

    [SerializeField]
    private Transform thirdPersonPivot;
    [SerializeField]
    private Transform firstPersonPivot;

    private void Awake()
    {
        source0 = gameObject.AddComponent<AudioSource>();
        source1 = gameObject.AddComponent<AudioSource>();
        body = GetComponent<Rigidbody>();
    }

    private void Test()
    {
        clip[0].CrossFade(source0, source1);
        ExtensionMethods.CrossFade(clip[0], source0, source1);

        body.MoveTowards(towards, moveSpeed);
        ExtensionMethods.MoveTowards(body, towards, lookSpeed);
    }

    private void Update()
    {
        body.MovementMethod(moveSpeed);
        body.ThirdPersonControl(thirdPersonPivot, lookSpeed);
    }
}
