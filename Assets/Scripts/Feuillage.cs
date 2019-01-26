using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feuillage : MonoBehaviour
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
        if (other.tag=="Sweet") {
            other.GetComponent<PlayerController>().SetSurFeuillage(true);
        }else if (other.tag == "Loup") {
            other.GetComponent<Loup>().SetSurFeuillage(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Sweet") {
            other.GetComponent<PlayerController>().SetSurFeuillage(false);
        }else if (other.tag == "Loup") {
            other.GetComponent<Loup>().SetSurFeuillage(false);
        }
    }
}
