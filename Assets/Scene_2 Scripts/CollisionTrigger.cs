using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour {

    private BoxCollider2D playerCollider;

    [SerializeField]
    private BoxCollider2D platformCollider;

    [SerializeField]
    private BoxCollider2D platformTrigger;
    // Use this for initialization
    void Start() {
        playerCollider = GameObject.Find("jhonny").GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "jhonny")  {
            //Ignore the collision between the 2 colliders when the player
            // Enters the collision
            Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name == "jhonny")
        {
            //Ignore the collision between the 2 colliders when the player
            // Enters the collision
            Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
        }
    }
}
