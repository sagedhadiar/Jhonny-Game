using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController {

    private IEnemyState currentState;

    public GameObject Target { get; set; }

    [SerializeField]
    private float meleeRange;

    [SerializeField]
    private float throwRange;

    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange; 
            }
            return false;
        }
    }

    public bool InThrowRange {
        get {
            if(Target != null) {
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;
            }
            return false;
        }
    }

    public override bool isDead {
        get
        {
            return health <= 0;
        }
           
    }

    // Use this for initialization
    public override void Start() {

        base.Start();

        PlayerController.Instance.Dead += new DeadEventHandler(RemoveTarget);

        ChangeState(new IdleState());
    }

    // Update is called once per frame
    void Update () {

        // If is not dead then we execute the current state and lookAtTarget
        if (!isDead) {

            //making sure if take a damage then he can trun arround if the player jump on the other side
            if (!TakingDamage) {
                currentState.Execute();
            }
            LookAtTarget();
        }

    }

    //AFter killing the player let the enemy return to patrol state
    public void RemoveTarget() {
        Target = null;

        ChangeState(new PatrolState());
    }

    void LookAtTarget() {

        if (Target != null) {
            //Get direction >0 or <0 
            //if <0 then on the left of me
            //if >0 then on the right of me
            // Target.transform.position.x is the position of the player
            // transform.position.x is the position of the enemy
            float xDir = Target.transform.position.x - transform.position.x;

            if (xDir < 0 && facingRight || xDir > 0 && !facingRight) {
                ChangeDirection();
            }
        }
    }

    public void ChangeState(IEnemyState newState) {
        if(currentState != null) {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }

    public void Move() {

        if (!Attack) {
            MyAnimator.SetFloat("speed", 1);

            transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));
        }
       
    }

    //Get the current direction
    public Vector2 GetDirection() {
        return facingRight ? Vector2.right : Vector2.left;
    }

    //If the enemy collides with an object
    public override void OnTriggerEnter2D(Collider2D other) {

        //calls the base on trigger enter
        base.OnTriggerEnter2D(other);

        //Calls OnTriggerEnter on the current state
        currentState.OnTriggerEnter(other);
    }

    //Makes the enemy takes damage
    public override IEnumerator TakeDamage() {

        health -= 10;

        //if the enemy is not dead then play the enemy-damage animation
        if (!isDead) {
            MyAnimator.SetTrigger("damage");
        }
        else {
            MyAnimator.SetTrigger("death");
            yield return null;
        }
    }

    public override void Death() {

        Destroy(gameObject);
        
    }


    //If we want to make the enemy respawn after a certain of time we can call this method instead of the upper method
    //public override void Death() {

    //    MyAnimator.ResetTrigger("death");

    //    MyAnimator.SetTrigger("idle");

    //    health = 30;

    //    transform.position = startPos;
    //}
}
