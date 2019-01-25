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
    float distance_too_far = 50.0f;
    private int state;

    //noise pos
    float[] noise_precisions = { Mathf.PI, 0.5f * Mathf.PI, 0.25f * Mathf.PI, 0.5f*Mathf.PI, 0.0f};
    Vector3 last_noise_pos = new Vector3(0.0f, 0.0f, 0.0f);
    float noise_precision = Mathf.PI;


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

        //the wolf walk randomly following last noise
        else if (state < 3)
        {
            if (timer <= 0)
            {
                WanderAround();
                timer = time;
            }
        }

        //teleport the wolf if too far
        float dist_sweety = Vector3.Distance(transform.position, sweet.transform.position);
        if(dist_sweety > distance_too_far)
        {
            float theta = Random.Range(0f, 2 * Mathf.PI);
            Vector3 new_pos = sweet.transform.position + distances[0] * new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
            transform.position = new_pos;
        }

    }

    void HearNoise()
    {
        noise_precision = noise_precisions[state];
        last_noise_pos = sweet.transform.position;
    }

    //se balader
    void WanderAround()
    {
        int action_to_do = Random.Range(0, 2);
        //it starts walking again
        if (action_to_do == 1)
        {
            //random theta noise
            float theta_rand = Random.Range(-noise_precision,noise_precision);
            //initial angle with position
            float theta = Vector3.Angle(sweet.transform.position - transform.position, Vector3.right) + theta_rand;
            float delta = Random.Range(0.5f, 1.5f);
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
