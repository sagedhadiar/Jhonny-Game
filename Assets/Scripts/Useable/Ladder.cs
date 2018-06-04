using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IUseable {

    [SerializeField]
    private Collider2D platformCollider;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Use() {

        if (PlayerController.Instance.OnLadder) {
            // we need to stop climbing
            UseLadder(false, 1, 0, 1, "land");
        }
        else {
            // we need to start climbing
            UseLadder(true, 0, 1, 0, "reset");
            Physics2D.IgnoreCollision(PlayerController.Instance.GetComponent<Collider2D>(), platformCollider, true);
        }
    }

    public void UseAndroid()  {
        
        UseLadderAndroid(true, 0, 1, 0, "reset");
        Physics2D.IgnoreCollision(PlayerController.Instance.GetComponent<Collider2D>(), platformCollider, true);
        
    }

    private void UseLadder(bool onLadder, int gravity, int weight, int animSpeed, string trigger) {

        PlayerController.Instance.OnLadder = onLadder;
        PlayerController.Instance.MyRigidBody.gravityScale = gravity;
        PlayerController.Instance.MyAnimator.SetLayerWeight(2, weight);
        PlayerController.Instance.MyAnimator.speed = animSpeed;
        PlayerController.Instance.MyAnimator.SetTrigger(trigger);

    }
    private void UseLadderAndroid(bool onLadder, int gravity, int weight, int animSpeed, string trigger)
    {

        PlayerController.Instance.OnLadder = onLadder;
        PlayerController.Instance.MyRigidBody.gravityScale = gravity;
        PlayerController.Instance.MyAnimator.SetLayerWeight(2, weight);
        PlayerController.Instance.MyAnimator.speed = animSpeed;
        PlayerController.Instance.MyAnimator.SetTrigger(trigger);

    }


    private void OnTriggerExit2D(Collider2D other) {

        if (other.tag == "jhonny"){
            UseLadder(false, 1, 0, 1, "land");
            Physics2D.IgnoreCollision(PlayerController.Instance.GetComponent<Collider2D>(), platformCollider, false);
        }
    }
}
