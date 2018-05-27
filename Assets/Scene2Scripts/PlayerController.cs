using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeadEventHandler();
public class PlayerController : CharacterController
{
    private static PlayerController instance;
    public static PlayerController Instance

    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<PlayerController>();

            return instance;
        }
    }

    public event DeadEventHandler Dead;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    private bool immortal = false;

    [SerializeField]
    private float immortalTime;

    private float direction;

    private bool move;

    private float btnHorizontal;

    private SpriteRenderer spriteRenderer;

    private Vector3 startPos;

    public Rigidbody2D MyRigidBody { get; set; }
   
    public bool Slide { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

    public override bool IsDead {
        get
        {
            if (healthStat.CurrentVal <= 0) { 
                OnDead();
            }

            return healthStat.CurrentVal <= 0;
        }
    }

    //Determine what ground is because some wil not be considered as ground
    [SerializeField]
    private LayerMask whatIsGround;

	// Use this for initialization
	public override void Start () {
        base.Start();

        startPos = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();

        //Reference to myrigidBody of the player
        MyRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update(){
        
        //after death the player can not move
        if (!TakingDamage && !IsDead) {

            if (transform.position.y <= -25f)  { 
                Death();
            }

            HandleInput();
        } 
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //after death the player can not move
        if (!TakingDamage && !IsDead) {

            //return 0 or 1
            float horizontal = Input.GetAxis("Horizontal");

            OnGround = IsGrounded();

            //Using btn move
            if (move) {

                //To decrease the acceleration while moving using buttons
                this.btnHorizontal = Mathf.Lerp(btnHorizontal, direction, Time.deltaTime * 2);

                //Call the move method
                HandleMovement(btnHorizontal);

                //Call the flip method
                Flip(direction);

            }
            else {
                HandleMovement(horizontal);

                Flip(horizontal);
            }

            HandleLayers();
        }
    }

    public void OnDead() {

        if (Dead != null)
        {
            Dead();
        }

    }

    private void HandleMovement(float horizontal){
        if(MyRigidBody.velocity.y < 0){
            MyAnimator.SetBool("land", true);
        }

        if(!Attack && !Slide && (OnGround || airControl)){
            MyRigidBody.velocity = new Vector2(horizontal * movementSpeed, MyRigidBody.velocity.y);
        }

        if(Jump && (MyRigidBody.velocity.y == 0)){
            MyRigidBody.AddForce(new Vector2(0, jumpForce));
        }

        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleInput(){
        if (Input.GetKeyDown(KeyCode.Space)){
            MyAnimator.SetTrigger("jump");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)){
            MyAnimator.SetTrigger("attack");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)){
            MyAnimator.SetTrigger("slide");
        }

        if (Input.GetKeyDown(KeyCode.V)) {
            MyAnimator.SetTrigger("throw");
        }

    }
    private void Flip(float horizontal){
        if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight){
            ChangeDirection();
        }
    }

    private bool IsGrounded(){
        //if velocity less than zero then falling and if equal to zero then not moving 
        if( MyRigidBody.velocity.y <= 0){
            foreach(Transform point in groundPoints) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for(int i = 0; i < colliders.Length; i++){
                    //if this collider is not the player because the script is on the player
                    if(colliders[i].gameObject != gameObject){
                        return true;
                    }
                }
            }
        }
        return false;
    }

    //used for jump Animation
    private void HandleLayers(){
        if (!OnGround){
            MyAnimator.SetLayerWeight(1, 1);
        }
        else{
            MyAnimator.SetLayerWeight(1, 0);
        }
    }

    public override void ThrowKnife(int value){
        if (!OnGround && value == 1 || OnGround && value == 0){
            base.ThrowKnife(value);
        }
    }


    private IEnumerator IndicateImmortal()
    {

        for (int i = 0; i < 5; i++) { 

            spriteRenderer.enabled = false;

            yield return new WaitForSeconds(.1f);

            spriteRenderer.enabled = true;

            yield return new WaitForSeconds(.1f);

        }


    }

    public override IEnumerator TakeDamage() {

        if (!immortal) {
            healthStat.CurrentVal -= 10;

            if (!IsDead)  {
                MyAnimator.SetTrigger("damage");

                immortal = true;

                StartCoroutine(IndicateImmortal());

                yield return new WaitForSeconds(immortalTime);

                immortal = false;
            }
            else {
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("death");
            }
        }
    }

    public override void Death() {

        MyRigidBody.velocity = Vector2.zero; 

        MyAnimator.SetTrigger("idle");

        healthStat.CurrentVal = healthStat.MaxVal;

        transform.position = startPos;
    }

    public void BtnJump() {
        MyAnimator.SetTrigger("jump");
        Jump = true;
    }
    public void BtnAttack() {
        MyAnimator.SetTrigger("attack");
    }
    public void BtnThrow() {
        MyAnimator.SetTrigger("throw"); 
    }
    public void BtnSlide() {
        MyAnimator.SetTrigger("slide");
    }
    public void BtnMove(float direction) {

        this.direction = direction;
        move = true;

    }

    public void BtnStopMove() {
        this.direction = 0;
        this.btnHorizontal = 0;
        this.move = false;
    }
}
