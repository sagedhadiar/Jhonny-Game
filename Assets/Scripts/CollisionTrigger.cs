using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour {

    [SerializeField]
    private BoxCollider2D platformCollider;

    //Platform Collider
    [SerializeField]
    private BoxCollider2D platformTrigger;
    // Use this for initialization
    void Start() {
        Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);
    }

    // Update is called once per frame
    void Update() {

    }

    //If it collides with something
    void OnTriggerEnter2D(Collider2D other) {

        //If the player collides with platform
        if (other.gameObject.tag == "jhonny" || other.gameObject.name == "Enemy") {

            //Then ignore collision
            //Ignore the collision between the 2 colliders when the player
            // Enters the collision
            Physics2D.IgnoreCollision(platformCollider, other, true);
        }
    }

    //When a trigger collision stops
    void OnTriggerExit2D(Collider2D other) {

        //If the player stop colliding 
        if (other.gameObject.tag == "jhonny" || other.gameObject.name == "Enemy") {
            //Ignore the collision between the 2 colliders when the player
            // Enters the collision
            Physics2D.IgnoreCollision(platformCollider, other, false);
        }
    }
}
