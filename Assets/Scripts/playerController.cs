using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerPhysics))]
public class playerController : MonoBehaviour {

    //Player Handling
    public float gravity = 20;
    public float speed = 8;
    public float acceleration = 30;
    public float jumpHeight = 12;

    private float currentSpeed;
    private float targetSpeed;
    private Vector2 amountToMove;

    private PlayerPhysics playerPhysics;
	// Use this for initialization
	void Start () {
        playerPhysics = GetComponent<PlayerPhysics>();

    }
	
	// Update is called once per frame
	void Update () {

        if (playerPhysics.movementStopped){
            targetSpeed = 0;
            currentSpeed = 0;
        }
        // Input
        targetSpeed = Input.GetAxisRaw("Horizontal") * speed;
        currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

        if (playerPhysics.grounded){
            amountToMove.y = 0;

            //Jump
            if (Input.GetButtonDown("Jump")){
                amountToMove.y = jumpHeight;
            }
        }

        amountToMove.x = currentSpeed;
        amountToMove.y -= gravity * Time.deltaTime;
        playerPhysics.Move(amountToMove * Time.deltaTime);
    }

    // Increase curspeed towards target by speed
    private float IncrementTowards(float curspeed, float target, float acc)
    {
        if(curspeed == target){
            return curspeed;
        }
        else{
            //must curspeed be increased or decreased to get closer to target
            float dir = Mathf.Sign(target - curspeed);
            curspeed += acc * Time.deltaTime * dir;
            //if curspeed has now passed target then return target, otherwise return curspeed;
            return (dir == Mathf.Sign(target - curspeed)) ? curspeed : target;
        }
    }
}
