using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManagerPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 100);
    }
}
