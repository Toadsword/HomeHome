using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buisson : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Sweet") {
            other.GetComponent<PlayerController>().surBuisson = true;
        } else if (other.tag == "Loup") {
            other.GetComponent<Loup>().surFeuillage = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Sweet") {
            other.GetComponent<PlayerController>().surBuisson = false;
        } else if (other.tag == "Loup") {
            other.GetComponent<Loup>().surFeuillage = false;
        }
    }

}
