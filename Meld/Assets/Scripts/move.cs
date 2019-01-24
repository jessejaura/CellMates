﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float speed = 1;
    public float topSpeed = 2;
    private Rigidbody cube;
    Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        cube = GetComponent<Rigidbody>();
        cube.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis ("Horizontal") *20;
        float vertical = Input.GetAxis ("Vertical")* 20;

        Vector3 movement = new Vector3 (horizontal, 0.0f, vertical);

        //cube.AddForce(movement * speed);

        cube.velocity = movement * speed;
        
        if (cube.velocity.magnitude > topSpeed)
            cube.velocity = cube.velocity.normalized * topSpeed;
        //Vector3 vel = cube.velocity;
        //vel.x = horizontal;
        //vel.y = vertical;

//        cube.velocity = vel;
    }
}
