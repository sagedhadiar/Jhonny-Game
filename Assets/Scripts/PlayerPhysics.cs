using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour {

    public LayerMask collisionMask;

    private BoxCollider colliders;
    private Vector3 size;
    private Vector3 center;

    private float skin = .005f;

    [HideInInspector]
    public bool grounded;

    [HideInInspector]
    public bool movementStopped;
    Ray ray;
    RaycastHit hit;
    public void Start(){

        colliders = GetComponent<BoxCollider>();
        size = colliders.size;
        center = colliders.center;
    }

    public void Move(Vector2 moveAmount){
        float deltaX = moveAmount.x;
        float deltaY = moveAmount.y;
        Vector2 position = transform.position;

        //Check collisions above and below
        grounded = false;
        for(int i = 0; i<3; i++){
            float dir = Mathf.Sign(deltaY);
            // Left, Center and then rightmost point of collider
            float x = (position.x + center.x - size.x/2) + size.x/2 * i;
            // Bottom of collider
            float y = position.y + center.y  + size.y / 2 * dir;

            ray = new Ray(new Vector2(x, y), new Vector2(0, dir));
            
            if (Physics.Raycast(ray, out hit, Mathf.Abs(deltaY) + skin, collisionMask)) {
                //Get Distance between player and ground
                float dist = Vector3.Distance(ray.origin, hit.point);

                //Stop player's downwards movement after coming within skin width of a collider
                if (dist > skin){
                    deltaY = dist * dir - skin * dir;
                }
                else{
                    deltaY = 0;
                }
                grounded = true;
                break;
            }
        }

        //Check collisions left and right
        movementStopped = false;
        for (int i = 0; i < 3; i++)
        {
            float dir = Mathf.Sign(deltaX);
            // Left, Center and then rightmost point of collider
            float x = position.x + center.x + size.x / 2 * dir;
            // Bottom of collider
            float y = position.y + center.y - size.y / 2 + size.y / 2 * i;

            ray = new Ray(new Vector2(x, y), new Vector2(dir, 0));

            if (Physics.Raycast(ray, out hit, Mathf.Abs(deltaX) + skin, collisionMask))
            {
                //Get Distance between player and ground
                float dist = Vector3.Distance(ray.origin, hit.point);

                //Stop player's downwards movement after coming within skin width of a collider
                if (dist > skin)
                {
                    deltaX = dist * dir - skin * dir;
                }
                else
                {
                    deltaX = 0;
                }
                movementStopped = true;
                break;
            }
        }

        Vector2 finalTransform = new Vector2(deltaX, deltaY);
        transform.Translate(finalTransform);
    }
}
