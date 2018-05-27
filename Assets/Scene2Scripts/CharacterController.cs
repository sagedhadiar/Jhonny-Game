using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterController : MonoBehaviour {

    [SerializeField]
    protected Transform KnifePos;

    //The knife Prefab, this is used for instantiating a knife
    [SerializeField]
    protected GameObject KnifePrefab;

    [SerializeField]
    protected float movementSpeed;

    //Indicates if the character is facing right
    protected bool facingRight;

    public bool Attack { get; set; }

    public bool TakingDamage { get; set; }

    public abstract void Death();

    //Character's Health
    [SerializeField]
    protected int health;

    //Character's Sword Collider
    [SerializeField]
    private EdgeCollider2D swordCollider;

    //A list of damage sources(that can damage the character)
    [SerializeField]
    private List<string> damageSources;

    public abstract bool IsDead { get; }

    [SerializeField]
    private string knifeTag;

    // Give the right position -->
    private int KnifeRightDirection;

    //Give the left position <--
    private int KnifeLeftDirection;

    public Animator MyAnimator { get; private set; }

    public EdgeCollider2D SwordCollider {
        get
        {
            return swordCollider;
        }
    }

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
    }

    // Update is called once per frame
    void Update() {

    }

    public abstract IEnumerator TakeDamage();

    public void ChangeDirection() {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public virtual void ThrowKnife(int value) {
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
        //Using 2 tags revent the player from hitting himself
        if (damageSources.Contains(other.tag)) {
            StartCoroutine(TakeDamage());
        }

    }

    

}
