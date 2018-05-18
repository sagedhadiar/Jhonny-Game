using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    [SerializeField]
    private float movementSpeed;

    private bool facingRight;

    private Animator myAnimator;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private GameObject KnifePrefab;

    [SerializeField]
    private Transform KnifePos;

    public Rigidbody2D MyRigidBody { get; set; }
    public bool Attack { get; set; }
    public bool Slide { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

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

    //Determine what ground is because some wil not be considered as ground
    [SerializeField]
    private LayerMask whatIsGround;

	// Use this for initialization
	void Start () {
        facingRight = true;

        //Reference to myrigidBody of the player
        MyRigidBody = GetComponent<Rigidbody2D>();

        myAnimator = GetComponent<Animator>();

    }

    void Update(){
        HandleInput();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //return 0 or 1
        float horizontal = Input.GetAxis("Horizontal");

        OnGround = IsGrounded();

        HandleMovement(horizontal);

        Flip(horizontal);

        HandleLayers();
    }

    private void HandleMovement(float horizontal){
        if(MyRigidBody.velocity.y < 0){
            myAnimator.SetBool("land", true);
        }

        if(!Attack && !Slide && (OnGround || airControl)){
            MyRigidBody.velocity = new Vector2(horizontal * movementSpeed, MyRigidBody.velocity.y);
        }

        if(Jump && (MyRigidBody.velocity.y == 0)){
            MyRigidBody.AddForce(new Vector2(0, jumpForce));
        }

        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleInput(){
        if (Input.GetKeyDown(KeyCode.Space)){
            myAnimator.SetTrigger("jump");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)){
            myAnimator.SetTrigger("attack");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)){
            myAnimator.SetTrigger("slide");
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            myAnimator.SetTrigger("throw");
        }

    }
    private void Flip(float horizontal){
        if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight){
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
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
            myAnimator.SetLayerWeight(1, 1);
        }
        else{
            myAnimator.SetLayerWeight(1, 0);
        }
    }

    public void ThrowKnife(int value){
        if (!OnGround && value == 1 || OnGround && value == 0)
        {

            if (facingRight) {
                GameObject tmp = (GameObject)Instantiate(KnifePrefab, KnifePos.position, Quaternion.Euler(new Vector3(0, 0, -90)));
                tmp.GetComponent<Knife>().Initialize(Vector2.right);
            }
            else {
                GameObject tmp = (GameObject)Instantiate(KnifePrefab, KnifePos.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                tmp.GetComponent<Knife>().Initialize(Vector2.left);
            }
        }
    }
}
