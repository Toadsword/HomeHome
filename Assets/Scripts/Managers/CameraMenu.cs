using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenu : MonoBehaviour
{

    private int timer = 0;
    private float theta_max = Mathf.PI / 20.0f;
    private int time = 500;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += 1;
        timer = timer % (2 * time);

        //compute the current d
        float d = timer;
        if(timer>time)
        {
            d = 2 * time - timer;
        }
        float d_ = (d - time / 2)/ (float)time;

        this.transform.rotation = Quaternion.Euler(0,0, d_ * theta_max*180.0f/Mathf.PI);
    }
}
