using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loup : MonoBehaviour
{
	private Rigidbody2D rigid;
    
    //distance of the new target point from the current wolf position
    [SerializeField]
    private int time = 10;

    //speed of the wolf
    [SerializeField]
    private float walk_speed = 1.0f;
    [SerializeField]
    private float run_speed = 2.0f;

    [SerializeField]
    private GameObject sweet;

    //distances and state
    float[] distances = {30.0f, 20.0f, 10.0f, 5.0f};
    private int state;

    //timer
    private int timer = 0;

    //current direction of the wolf
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
		rigid = GetComponent<Rigidbody2D>();
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        --timer;
        
        UpdateState();

        //the wolf attack
        if (state ==3)
        {
            direction = Vector3.Normalize(sweet.transform.position - transform.position);
            rigid.velocity = run_speed * direction;
        }

        //the wolf walk randomly
        else if (state < 3)
        {
            if (timer <= 0)
            {
                WanderAround();
                timer = time;
            }
        }


    }

    //se balader
    void WanderAround()
    {
        int action_to_do = Random.Range(0, 2);
        //it starts walking again
        if (action_to_do == 1)
        {
            //update direction
            float theta = Random.Range(0f, 2 * Mathf.PI);
            float delta = Random.Range(0.8f, 1.2f);
            direction = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
            rigid.velocity = walk_speed * direction;
        }

        else if (action_to_do == 0)
        {
            //it makes a little break
            rigid.velocity = new Vector3(0, 0, 0);
        }
    }

    void UpdateState()
    {
        float dist_sweety = Vector3.Distance(transform.position, sweet.transform.position);

        if (state + 1 < distances.Length && dist_sweety < distances[state + 1])
            state += 1;
        else if (state - 1 >= 0 && dist_sweety > distances[state - 1])
            state -= 1;
    }
}
