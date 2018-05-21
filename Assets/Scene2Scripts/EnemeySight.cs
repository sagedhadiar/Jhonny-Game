using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeySight : MonoBehaviour {

    [SerializeField]
    private EnemyController enemy;
	// Use this for initialization

	void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "jhonny") {
            enemy.Target = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "jhonny") {
            enemy.Target = null;
        }
    }
}
