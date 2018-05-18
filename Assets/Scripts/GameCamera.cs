using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour {

    private Transform target;
    private float trackSpeed = 10;

    public void SetTarget(Transform targ){
        target = targ;
    }
    
    void LateUpdate(){
        if (target){
            float x = IncrementTowards(transform.position.x, target.position.x, trackSpeed);
            float y = IncrementTowards(transform.position.y, target.position.y, trackSpeed);
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }

    // Increase curspeed towards target by speed
    private float IncrementTowards(float curspeed, float target, float acc)
    {
        if (curspeed == target)
        {
            return curspeed;
        }
        else
        {
            //must curspeed be increased or decreasedto get closer to target
            float dir = Mathf.Sign(target - curspeed);
            curspeed += acc * Time.deltaTime * dir;
            //if curspeed has now passed target then return target, otherwise return curspeed;
            return (dir == Mathf.Sign(target - curspeed)) ? curspeed : target;
        }
    }
}
