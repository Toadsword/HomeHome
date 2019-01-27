using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenuShake : MonoBehaviour
{

    float timer_shaking = 0;
    Vector3 posInit;

    float f1 = 0.15f;
    float f2 = 0.2f;
    float rayon = 270;


    // Start is called before the first frame update
    void Start()
    {
        timer_shaking = 0;
        posInit = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer_shaking += Time.deltaTime;
        transform.position = posInit + new Vector3(Mathf.Cos(timer_shaking*f1)-1,0, 0) * rayon;
    }
}
