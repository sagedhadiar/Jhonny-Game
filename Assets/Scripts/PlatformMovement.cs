using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformMovement : MonoBehaviour {

    //Initially our current position
    //private Vector3 currentPos;

    private Vector3 posA;

    private Vector3 posB;

    //if current pos equal to A then the next is B
    private Vector3 nexPos;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform childTransform;

    [SerializeField]
    private float xMax;

    [SerializeField]
    private float yMax;

    [SerializeField]
    private float xMin;

    [SerializeField]
    private float yMin;

    public enum myEnum // your custom enumeration
    {
        Vertical,
        Horizontal,
        Oblique
    };

    public myEnum moveDirection;

    public enum myEnum1 // your custom enumeration
    {
        RightOrUP,
        LeftOrDown
    };

    public myEnum1 movePos;

    // Use this for initialization
    void Start () {

        //myEnum.Item1 = "v";
        //currentPos = childTransform.localPosition;

        posA = moveDirection.ToString() == "Vertical" ? new Vector3(transform.localPosition.x, yMax) : moveDirection.ToString() == "Horizontal" ? new Vector3(xMax, transform.localPosition.y) : new Vector3(xMax, yMax);

        posB = moveDirection.ToString() == "Vertical" ? new Vector3(transform.localPosition.x, yMin) : moveDirection.ToString() == "Horizontal" ? new Vector3(xMin, transform.localPosition.y) : new Vector3(xMin, yMin);

        if (movePos.ToString() == "RightOrUP")
            nexPos = posA;
        else if(movePos.ToString() == "LeftOrDown")
            nexPos = posB;

    }
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    private void Move() {

       
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, nexPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.localPosition, nexPos) <= 0.1)
            ChangeDestination();

    }

    private void ChangeDestination() {
        nexPos = nexPos != posA ? posA : posB;
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if(other.gameObject.tag == "jhonny") {
            other.gameObject.layer = 11;
            other.transform.SetParent(childTransform);
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        other.transform.SetParent(null);
    }
}
