using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void DeadEventHandler();
public class PlayerController : CharacterController {

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

    private IUseable useable;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private bool airControl;

    private bool canMoveHorizontal;

    [SerializeField]
    private float jumpForce;

    private bool immortal = false;

    [SerializeField]
    private float immortalTime;

    private float direction;

    private bool move;

    private float btnHorizontal;

    private float btnVertical;

    private SpriteRenderer spriteRenderer;

    private Vector3 startPos;

    public Rigidbody2D MyRigidBody { get; set; }

    public bool OnLadder { get; set; }

    [SerializeField]
    private float climbSpeed;
   
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

    public bool IsFalling
    {
        get
        {
            return MyRigidBody.velocity.y < 0;
        }
    }

    private int extraJumpValue;

    [SerializeField]
    private int extraJump;

    [SerializeField]
    private bool onMediumPlatform = false;

    //Determine what ground is because some wil not be considered as ground
    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private GameObject[] buttons;

    [SerializeField]
    private CircleCollider2D healthCollider;

    [SerializeField]
    private Rigidbody2D healthBody;

    // Use this for initialization
    public override void Start () {
        base.Start();

        OnLadder = false;

        extraJumpValue = extraJump;

        startPos = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();

        //Reference to myrigidBody of the player
        MyRigidBody = GetComponent<Rigidbody2D>();

        //Collider with the HealthCoin
        
        healthCollider.isTrigger = true;
        healthBody.gravityScale = 0;
    }

    void Update(){

        if (healthCollider != null) {

            if (healthStat.CurrentVal == healthStat.MaxVal)
            {
                healthCollider.isTrigger = true;
                healthBody.gravityScale = 0;
            }
            else
            {
                healthCollider.isTrigger = false;
                healthBody.gravityScale = 1;
            }
        }

        StayCollliding();

        //after death the player can not move
        if (!TakingDamage && !IsDead) {

            if (transform.position.y <= -25f)  { 
                Death();
            }

            HandleInput();
        }
     
            foreach (GameObject butTag in buttons)
            {
                if (useable != null)  {
                    if (butTag.tag == "UpButton" || butTag.tag == "DownButton") {
                        butTag.SetActive(true);
                    }
                    else {
                        butTag.SetActive(false);
                    }
                }
                else {
                    if (butTag.tag == "UpButton" || butTag.tag == "DownButton") {
                        butTag.SetActive(false);
                    }
                    else {
                        butTag.SetActive(true);
                    }
                }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //after death the player can not move
        if (!TakingDamage && !IsDead) {

            //return 0 or 1
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            OnGround = IsGrounded();

            //Using btn move
            if (move) {

                canMoveButtons(canMoveHorizontal);

            }
            else {
                HandleMovement(horizontal, vertical);

                Flip(horizontal);
            }

            HandleLayers();
        }
    }

    //public void OnDead() {

    //    Dead?.Invoke();

    //}

    public void OnDead()
    {
        if (Dead != null)
        {
            Dead();
        }
    }

    private void HandleMovement(float horizontal, float vertical){
        if(IsFalling){
            //Falling Layer
            gameObject.layer = 12;
            MyAnimator.SetBool("land", true);
        }

        if(OnGround && extraJumpValue == 0) {
                extraJumpValue = extraJump;
        }

        if (!Attack && !Slide && (OnGround || airControl)){
            MyRigidBody.velocity = new Vector2(horizontal * movementSpeed, MyRigidBody.velocity.y);
        }

        if(Jump && !OnLadder && extraJumpValue >0) {
            if(extraJumpValue == 2)
                MyRigidBody.AddForce(new Vector2(0, jumpForce));
            else if (extraJumpValue == 1)
                MyRigidBody.AddForce(new Vector2(0, jumpForce ));
            extraJumpValue--;
            Jump = false;
        }

        if (OnLadder) {
            MyAnimator.speed = vertical !=0 ? Mathf.Abs(vertical) : Mathf.Abs(horizontal);
            MyRigidBody.velocity = new Vector2(horizontal * climbSpeed, vertical * climbSpeed);
        }

        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleInput(){
        if (Input.GetKeyDown(KeyCode.Space) && !OnLadder && !IsFalling){
            MyAnimator.SetTrigger("jump");
            Jump = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)){
            MyAnimator.SetTrigger("attack");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && MyRigidBody.velocity != Vector2.zero)
        {
            MyAnimator.SetTrigger("slide");
        }

        if (Input.GetKeyDown(KeyCode.V)) {
            MyAnimator.SetTrigger("throw");
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            Use();
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

        while (immortal) { 

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

    private void Use() {
        if(useable != null) {
            useable.Use();
        }
    }

    public void BtnJump() {
        if (!OnLadder && !IsFalling) {
            MyAnimator.SetTrigger("jump");
            Jump = true;
        }
    }
    public void BtnAttack() {
        MyAnimator.SetTrigger("attack");
    }
    public void BtnThrow() {
        MyAnimator.SetTrigger("throw"); 
    }
    public void BtnSlide() {
        if(MyRigidBody.velocity != Vector2.zero)
            MyAnimator.SetTrigger("slide");
    }
    public void BtnMove(float direction) {
        this.direction = direction;
        this.canMoveHorizontal = true;
        move = true;
    }

    public void BtnMoveVertical(float direction)
    {
        if (useable != null) {
            if ((onMediumPlatform && direction != 1) || (OnGround && !onMediumPlatform && direction != -1) || (!onMediumPlatform && !OnGround)) { 
                this.canMoveHorizontal = false;
                this.direction = direction;
                move = true;
                useable.UseAndroid();
            }
        }
       

    }

    public void BtnStopMove() {
        this.direction = 0;
        this.btnHorizontal = 0;
        this.btnVertical = 0;
        this.move = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Coin")  {
            GameManager.Instance.CollectedCoins++;
            Destroy(other.gameObject);
        }


        if (other.gameObject.tag == "Health" && healthStat.CurrentVal != healthStat.MaxVal) {
            healthStat.CurrentVal += 5;
            healthCollider = null;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Health" && healthStat.CurrentVal == healthStat.MaxVal) {
            other.collider.isTrigger = true;
            other.rigidbody.gravityScale = 0;
        }

        if (other.gameObject.tag == "MediumPlatform") {
            onMediumPlatform = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "MediumPlatform") {
            onMediumPlatform = false;
        }
    }

    public override void OnTriggerEnter2D(Collider2D other) {

        if(other.tag == "Useable") {
            useable = other.GetComponent<IUseable>();
        }
        collidersss.Add(other.tag);
        base.OnTriggerEnter2D(other);
    }

    private void OnTriggerExit2D(Collider2D other) {

        if (other.tag == "Useable") {
            useable = null;
        }
        collidersss.Remove(other.tag);
    }
    void StayCollliding() {
        if (collidersss.Contains("Thorn")) {
            StartCoroutine(TakeDamage());
        }
    }

    private void canMoveButtons(bool canMoveHorizontal) {
        //To decrease the acceleration while moving using buttons
        if (canMoveHorizontal) {
            this.btnHorizontal = Mathf.Lerp(btnHorizontal, direction, Time.deltaTime * 2);
            this.btnVertical = 0;
        }
        else {
            this.btnHorizontal = 0;
            this.btnVertical = Mathf.Lerp(direction, btnVertical, Time.deltaTime * 2);
        }

        //Call the move method
        HandleMovement(btnHorizontal, btnVertical);

        //Call the flip method
        Flip(direction);
    }


}
