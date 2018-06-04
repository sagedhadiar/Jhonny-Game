using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeJump : MonoBehaviour
{

    private Vector2 startTouchPosition, endTouchPosition;
    private Rigidbody2D rb;
    private bool jumpAllowed = false;
    private bool slideAllowed = false;
    private bool isTouching = false;

    // Use this for initialization
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update() {
        SwipeCheck();
    }

    private void FixedUpdate() {
        JumpIfAllowed();
        SlideIfAllowed();
    }

    private void SwipeCheck() {

        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0)) {
            isTouching = true;
            startTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0)) {
            isTouching = false;
        }
        #endregion

        #region Mobile Inputs
        //Debug.Log("Touches: " + Input.touches.Length);
        if (Input.touches.Length > 0) {
            if (Input.touches[0].phase == TouchPhase.Began) {
                isTouching = true;
                startTouchPosition = Input.touches[0].position;
            }

            if (Input.touches[0].phase == TouchPhase.Ended ) {
                isTouching = false;
            }
        }
        #endregion

        endTouchPosition = Vector2.zero;
        if (startTouchPosition != Vector2.zero)  {
            if (isTouching) {
                if (Input.touches.Length > 0)
                {
                    endTouchPosition = Input.touches[0].position - startTouchPosition;
                }
                else if (Input.GetMouseButton(0))
                    endTouchPosition = (Vector2)Input.mousePosition - startTouchPosition;
            }
        }

        if (endTouchPosition.magnitude > 125) {

            //which direction?
            float x = endTouchPosition.x;
            float y = endTouchPosition.y;

            if (Mathf.Abs(x) < Mathf.Abs(y)) {
                //Up or Down
                if (y < 0) {
                    slideAllowed = true;
                }
                else if( y>0) {
                    jumpAllowed = true;
                }
            }
           // jumpAllowed = slideAllowed = false;
        }

        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //    startTouchPosition = Input.GetTouch(0).position;

        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        //{
        //    endTouchPosition = Input.GetTouch(0).position;
        //    if (endTouchPosition.y > startTouchPosition.y && rb.velocity.y == 0)
        //        jumpAllowed = true;
        //    else if(endTouchPosition.y > startTouchPosition.y){
        //        slideAllowed = true;
        //    }
        //}
    }

    private void JumpIfAllowed()  {
        if (jumpAllowed) {
            if (!PlayerController.Instance.OnLadder && !PlayerController.Instance.IsFalling) {
                PlayerController.Instance.MyAnimator.SetTrigger("jump");
                PlayerController.Instance.Jump = true;
                jumpAllowed = false;
            }
        }
    }

    private void SlideIfAllowed() {
        if (slideAllowed) {
            if (PlayerController.Instance.MyRigidBody.velocity != Vector2.zero) {
                PlayerController.Instance.MyAnimator.SetTrigger("slide");
                slideAllowed = false;
            }
        }
    }

}
