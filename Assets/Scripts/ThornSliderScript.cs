using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornSliderScript : MonoBehaviour {

    private Vector3 posA;

    [SerializeField]
    private Vector3 nextPos;

    [SerializeField]
    private float moveUpDistance;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform childTransform;

    private SpriteRenderer spriteRenderer;

    private EdgeCollider2D edgeCollider;

    private bool showSpike;

    private bool canMove;

    // Use this for initialization
    void Start () {

        posA = transform.localPosition;

        nextPos = new Vector3(posA.x, posA.y + moveUpDistance);

        spriteRenderer = childTransform.GetComponent<SpriteRenderer>();

        edgeCollider = childTransform.GetComponent<EdgeCollider2D>();

        edgeCollider.enabled = false;

        spriteRenderer.sortingOrder = -10;

        showSpike = false;

        canMove = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (canMove) {
            if (showSpike)
                moveUp();
            else
                moveDown();
        }

    }

    void moveUp() {

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, nextPos, speed * Time.deltaTime);

    }

    void moveDown() {

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, posA , speed * Time.deltaTime);

    }


    void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.tag == "jhonny") {

            spriteRenderer.sortingOrder = 1;

            showSpike = true;

            canMove = true;

            edgeCollider.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)  {

        spriteRenderer.sortingOrder = -10;

        showSpike = false;

        canMove = true;

        edgeCollider.enabled = false;
    }




}
