using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController {

    //The enemy's current state
    //changing this will change the enemys behaviour
    private IEnemyState currentState;

    //The enemy's target
    public GameObject Target { get; set; }

    //The enemy's melee range, at what range does the enemy need to use the sword
    [SerializeField]
    private float meleeRange;

    // The enemy's throw range, how far can it start throwing knifes
    [SerializeField]
    private float throwRange;

    private Vector3 startPos;

    [SerializeField]
    private Transform leftEdge;

    [SerializeField]
    private Transform rightEdge;

    private Canvas healthCanvas;

    private bool dropItem = true;

    [SerializeField]
    private int decEnemyDamage;

    //Indicates if the enemy is in melee range
    public bool InMeleeRange {
        get {
            if (Target != null) {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange; 
            }
            return false;
        }
    }

    //Indicates if the enemy is in throw range
    public bool InThrowRange {
        get {
            if(Target != null) {
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;
            }
            return false;
        }
    }

    //Indicates if the enemy is dead
    public override bool IsDead {
        get {
            return healthStat.CurrentVal <= 0;
        }
           
    }

    // Use this for initialization
    public override void Start() {

        //this.startPos = transform.position;

        //Calls the base start
        base.Start();

        //Makes the RemoveTarget function listen to the player's Dead Event
        PlayerController.Instance.Dead += new DeadEventHandler(RemoveTarget);

        //Sets the enemy in idle state
        ChangeState(new IdleState());

        healthCanvas = transform.GetComponentInChildren<Canvas>();
    }

    // Update is called once per frame
    void Update () {

        // If is not dead then we execute the current state and lookAtTarget
        //If the enemy is alive
        if (!IsDead) {

            //making sure if take a damage then he can trun arround if the player jump on the other side
            //if we are not taking damage
            if (!TakingDamage) {

                //Execute the current state, this can make the enemy move or attack ...
                currentState.Execute();
            }

            //Make the enemy look at his target
            LookAtTarget();
        }

    }

    //After killing the player let the enemy return to patrol state
    //Removes the enemy's target
    public void RemoveTarget() {

        //Removes the target
        Target = null;

        //Changes the state to a patrol state
        ChangeState(new PatrolState());
    }

    //Makes the enemy look at the target
    void LookAtTarget() {
        //If we have target
        if (Target != null) {

            //Calculate the direction
            //if <0 then on the left of me
            //if >0 then on the right of me
            // Target.transform.position.x is the position of the player
            // transform.position.x is the position of the enemy
            float xDir = Target.transform.position.x - transform.position.x;

            //If we are turning the wromg way
            if (xDir < 0 && facingRight || xDir > 0 && !facingRight) {

                //look in the right direction
                ChangeDirection();
            }
        }
    }

    //Changes the enemy's state
    public void ChangeState(IEnemyState newState) {

        //If we have a current state
        if(currentState != null) {

            //Call the exit function on the state
            currentState.Exit();
        }

        //Sets the current state as the new State
        currentState = newState;

        //Calls the enter function on the current state
        currentState.Enter(this);
    }

    //Moves the enemy
    public void Move() {

        //If the enemy is not attacking
        if (!Attack) {

            //in order the enemy does get out of the platform while following the player
            if ((GetDirection().x > 0 && transform.position.x < rightEdge.position.x) || (GetDirection().x < 0 && transform.position.x > leftEdge.position.x) ){

                //Sets the speed to 1 to the player the run animation
                MyAnimator.SetFloat("speed", 1);

                //Moves the enemy in the correct direction
                transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));


            }
            else if (currentState is PatrolState) {
                ChangeDirection();
            }
            else if(currentState is RangedState) {
                Target = null;
                ChangeState(new IdleState());
            }
        }
       
    }

    //Gets the current direction
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

        if (!healthCanvas.isActiveAndEnabled) {
            healthCanvas.enabled = true;
        }

        //Reduces the health
        healthStat.CurrentVal -= decEnemyDamage;

        //if the enemy is not dead then play the enemy-damage animation
        if (!IsDead) {
            MyAnimator.SetTrigger("damage");
        }
        //If the enemy is dead then make sure that we play the dead animation 
        else {

            
            if (dropItem)  {
                GameObject coin = Instantiate(GameManager.Instance.CoinPrefab, new Vector3(transform.position.x, transform.position.y + 2), Quaternion.identity);
                Physics2D.IgnoreCollision(coin.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                dropItem = false;
            }
            MyAnimator.SetTrigger("death");
            GameManager.Instance.CollectedEnemyKilled--;
            yield return null;

        }
    }

    public override void Death() {
        Destroy(gameObject);
    }

    //Removes the enemy from the game
    //If we want to make the enemy respawn after a certain of time we can call this method instead of the upper method
    //public override void Death()
    //{

    //    dropItem = true;

    //    MyAnimator.ResetTrigger("death");

    //    MyAnimator.SetTrigger("idle");

    //    healthStat.CurrentVal = healthStat.MaxVal;

    //    transform.position = startPos;

    //    healthCanvas.enabled = false;
    //}
}
