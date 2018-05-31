using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterController : MonoBehaviour {

    [SerializeField]
    protected Transform KnifePos;

    [SerializeField]
    protected float movementSpeed;

    //Indicates if the character is facing right
    protected bool facingRight;

    //The knife Prefab, this is used for instantiating a knife
    [SerializeField]
    protected GameObject KnifePrefab;

    //Character's Health
    [SerializeField]
    protected Stat healthStat;

    //Indicates if the character is dead
    public abstract bool IsDead { get; }

    //Indicates if the character can attack
    public bool Attack { get; set; }

    //A reference to the character's animator
    public Animator MyAnimator { get; private set; }

    //Indicates if the character is taking damage
    public bool TakingDamage { get; set; }

    //Handles the character's death
    public abstract void Death();

    //Character's Sword Collider
    [SerializeField]
    private EdgeCollider2D swordCollider;

    //A list of damage sources(tags that can damage the character)
    [SerializeField]
    private List<string> damageSources;

    [SerializeField]
    private string knifeTag;

    // Give the right position -->
    private int KnifeRightDirection;

    //Give the left position <--
    private int KnifeLeftDirection;

    //Property for getting the swordCollider
    public EdgeCollider2D SwordCollider {
        get {
            return swordCollider;
        }
    }

    //Makes the character take damage
    public abstract IEnumerator TakeDamage();

    // Use this for initialization
    public virtual void Start() {

        facingRight = true;

        MyAnimator = GetComponent<Animator>();

        if (knifeTag == "PlayerKnife") {
            KnifeRightDirection = -90;
            KnifeLeftDirection = 90;
        }
        else {
            KnifeRightDirection = 0;
            KnifeLeftDirection = 180;
        }

        healthStat.Initialize();
    }

    // Update is called once per frame
    void Update() {

    }

    //Changes the characters direction
    public void ChangeDirection() {
        //Changes the  facingRight bool to its negative value
        facingRight = !facingRight;

        //Flips the character by changing the scale
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    //Throws the Knife
    public virtual void ThrowKnife(int value) {

        //If we are facing right then throw the knife to the right
        if (facingRight) {
            GameObject tmp = (GameObject)Instantiate(KnifePrefab, KnifePos.position, Quaternion.Euler(new Vector3(0, 0, KnifeRightDirection)));
            tmp.GetComponent<Knife>().Initialize(Vector2.right);
        }
        else {
            GameObject tmp = (GameObject)Instantiate(KnifePrefab, KnifePos.position, Quaternion.Euler(new Vector3(0, 0, KnifeLeftDirection)));
            tmp.GetComponent<Knife>().Initialize(Vector2.left);
        }
    }

    public void MeleeAttack() {
        SwordCollider.enabled = true;

    }

    public virtual void OnTriggerEnter2D(Collider2D other) {

        //Using 2 tags prevent the player from hitting himself
        if (damageSources.Contains(other.tag)) {
            StartCoroutine(TakeDamage());
        }

    }

    

}
