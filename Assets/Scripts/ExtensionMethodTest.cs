using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtensionMethodTest : MonoBehaviour
{
    public AudioClip[] clip;
    private AudioSource source0;
    private AudioSource source1;

    private Rigidbody body;
    private Vector3 towards;
    private float speed = 10;

    [SerializeField]
    private Transform pivot;
    [SerializeField]
    private Camera cam;

    private void Start()
    {
        source0 = gameObject.AddComponent<AudioSource>();
        source1 = gameObject.AddComponent<AudioSource>();
        body = GetComponent<Rigidbody>();
    }

    private void Test()
    {
        clip[0].CrossFade(source0, source1);
        ExtensionMethods.CrossFade(clip[0], source0, source1);

        body.MoveTowards(towards, speed);
        ExtensionMethods.MoveTowards(body, towards, speed);
    }

    private void Update()
    {
        body.MovementMethod(speed);
        body.ThirdPersonControl(pivot, speed);
    }
}
