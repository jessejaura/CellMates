using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NVIDIA.Flex;

public class move4 : MonoBehaviour
{
    public float speed = 1;
    public float topSpeed = 2;
    public float maxSeparation = 2;
    public float jump = 10;
    private Rigidbody rb;
    private ParticleGetter particleGetter;
    Vector3 movement;
    private GameObject player1;
    private GameObject player2;
    private GameObject membrane;
    private FlexClothActor membraneActor;

    public string inputHorizontal;
    public string inputVertical;
    public string player1Tag = "Player1";
    public string player2Tag = "Player2";
    public string membraneTag = "Membrane";

    public float jumpMagnitude = 20f;
    public float raycastDistance = 0.4f;
    bool grounded = true;
    bool playerGrounded = true;
    GameObject otherPlayer = null;
    move4 otherPlayersMove = null;
    RaycastHit[] hits = new RaycastHit[10];

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        player1 = GameObject.FindWithTag(player1Tag);
        player2 = GameObject.FindWithTag(player2Tag);
        // figure out which player this is so we can do separate groundedness calculation
        if (this == player1.GetComponent<move4>())
            otherPlayer = player2;
        else if (this == player2.GetComponent<move4>())
            otherPlayer = player1;
        if (otherPlayer == null)
            print("player: " + name);
        else
        otherPlayersMove = otherPlayer.GetComponent<move4>();
        membrane = GameObject.FindWithTag(membraneTag);
        membraneActor = membrane.GetComponent<FlexClothActor>();
        // particleGetter = membrane.GetComponent<ParticleGetter>();
 
        // this section is just for going through the FlexContainer's particle data
        // (might be useful later if we're directly manipulating particles)
        //if (particleGetter != null)
        //{
        //    FlexContainer.ParticleData pd = particleGetter.particleData;
        //    int cnt = 0;
        //    if (pd != null)
        //    {
        //        for (int i = 0; i < 10000; ++i)
        //        {
        //            Vector4 p0 = pd.GetParticle(i);
        //            if (p0.x != 0 || p0.y != 0 || p0.z != 0 || p0.w != 0)
        //            {
        //                print("particle " + i + " = " + p0);
        //                cnt++;
        //            }
        //        }
        //    }
        //    print("num particles: " + cnt);
        //}
        //else
        //{
        //    print("particle getter is null");
        //}

    }

    void Update()
    {
 
    }
    // Update is called once per frame, FixedUpdate less frequently                                              
    void FixedUpdate()
    {
        Vector3 velocity = rb.velocity;

        float horizontal = Input.GetAxis(inputHorizontal) * 20;
        float vertical = Input.GetAxis(inputVertical) * 20;
        if (horizontal != 0 || vertical != 0)
        {
            // print("Input for " + name);
            Vector3 movement = new Vector3(horizontal, 0.0f, vertical);
            // Controlling movement speed on the XZ plane
            movement = movement.normalized * speed;
            if (movement.magnitude > topSpeed)
                movement = movement.normalized * topSpeed;
            // This is to preserve Y movement so that gravity affects it properly
            movement.y = rb.velocity.y;
            rb.velocity = movement;
        }

        velocity = rb.velocity;
        Vector3 position = transform.position;
        var verticalSpeed = velocity.y;
        var verticalPosition = position.y;
 
        // Checking if we are above the ground, stop moving so we don't sink out of the blob
        var ray = new Ray(transform.position, Vector3.down);
        playerGrounded = Physics.Raycast(ray, raycastDistance);
        if (playerGrounded)
        {
             // TODO: figure out what the ray hit and filter it out if it's the FlexCloth
            int numhits = Physics.RaycastNonAlloc(ray, hits, raycastDistance);

            for (int i = 0; i < numhits; ++i)
            {
                 RaycastHit hit = hits[i];
                 print(name + ": ray hits " + hit.collider.gameObject.name);
            }
        }

        if (verticalPosition <= -1)
            // check if it hit the floor or not
        {
            verticalPosition = -1;
            position.y = verticalPosition;
            transform.position = position;
            if (verticalSpeed < 0)
            {
                // soften the bounce
                verticalSpeed = -verticalSpeed * 0.3f;
                velocity.y = verticalSpeed;
                velocity.x = velocity.x * 0.3f;
                velocity.z = velocity.z * 0.3f;
                rb.velocity = velocity;
            }
        }


     
        grounded = playerGrounded; // || otherPlayersMove.playerGrounded;      
        Vector3 player1position = player1.transform.position;
        Vector3 player2position = player2.transform.position;
        player1position.y = 0;
        player2position.y = 0;
        Vector3 avg = (transform.position + otherPlayer.transform.position) / 2;

        if (!grounded)
        {
            Vector3 dir = avg - transform.position;                                                           
            if (dir.y > 0)
                dir.y = 0;
            float magnitude = jumpMagnitude / 5;
            rb.AddForce(dir.normalized * magnitude);
            membrane.transform.position = avg;
            membraneActor.Teleport(membrane.transform.position, membrane.transform.rotation);
        }
        else if (Vector3.Distance(player1position, player2position) > maxSeparation)
        {
            float magnitude = jumpMagnitude;
            avg.y = jump;
            rb.AddForce((avg - transform.position).normalized * magnitude);
        }
        
    }
}
