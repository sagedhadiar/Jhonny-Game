using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{

    [SerializeField]
    private List<string> targetTag;

    void OnTriggerEnter2D(Collider2D other)
    {

        if (targetTag.Contains(other.tag)) {
            GetComponent<Collider2D>().enabled = false;
        }
    }

}
