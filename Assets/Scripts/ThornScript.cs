using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornScript : MonoBehaviour {

    public GameObject move;
    private void OnTriggerStay() {
        move.transform.position += move.transform.forward * Time.deltaTime;
      
    }
}
