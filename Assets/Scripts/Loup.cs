using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loup : MonoBehaviour
{
    public bool surFeuillage = false;

	private Rigidbody2D rigid;
    
    //distance of the new target point from the current wolf position
    [SerializeField]
    private int time = 10;

    //speed of the wolf
    [SerializeField]
    private float walk_speed = 1.0f;
    [SerializeField]
    private float run_speed = 2.0f;

    float current_speed = 0.0f;

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

    //house
    float x1_house = -3.5f;
    float x2_house = 3.5f;
    float y1_house = -1.0f;
    float y2_house = 5.5f;
   

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
        if (state == 3)
            RunToGirl();

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

        //move the wolf
        float alpha = 1.0f;
        if(surFeuillage)
            alpha = 0.5f;

        rigid.velocity = current_speed * direction * alpha;
    }

    void RunToGirl()
    {
        //deal with house ...
        if (CollidesHouse(transform.position, sweet.transform.position))
        {
            Debug.Log("oui");
            float dx_house = x2_house - x1_house;
            float dy_house = y2_house - y1_house;

            float infty_ = 10000.0f;
            Vector3[] points = new Vector3[]{new Vector3(x1_house, y1_house,0),
                                            new Vector3(x1_house, y2_house,0),
                                            new Vector3(x2_house, y2_house,0),
                                            new Vector3(x2_house, y1_house,0)};

            float[,] distance_table = new float[,]{{0,dy_house,infty_,dx_house},
                                                    {dy_house, 0, dx_house, infty_},
                                                    {infty_, dx_house, 0, dy_house},
                                                    {dx_house, infty_, dy_house, 0}};

            List<int> from_wolf = new List<int>();
            List<int> from_sweet = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if (!CollidesHouse(points[i], transform.position))
                    from_wolf.Add(i);
                if (!CollidesHouse(points[i],sweet.transform.position,0.2f))
                    from_sweet.Add(i);
            }

            float best_dist = infty_;
            int best_point = 0;

            for (int i=0; i<from_wolf.Count; ++i)
            {
                for(int j = 0; j<from_sweet.Count; ++j)
                {
                    float current_dist = distance_table[from_wolf[i],from_sweet[j]]
                        + Vector3.Distance(transform.position, points[from_wolf[i]])
                        + Vector3.Distance(sweet.transform.position, points[from_sweet[j]]);
                    if(current_dist<best_dist)
                    {
                        best_point = from_wolf[i];
                        best_dist = current_dist;
                    }
                }
            }
            direction = Vector3.Normalize(points[best_point] - transform.position);

        }
        else
        {
            direction = Vector3.Normalize(sweet.transform.position - transform.position);
        }

        current_speed = run_speed;
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
            current_speed =  walk_speed;
        }
        if(action_to_do==0)
        {
            current_speed = 0.0f;
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

    bool CollidesHouse(Vector3 a, Vector3 b, float radius = 0.5f)
    {
        RaycastHit2D r = Physics2D.CircleCast(a, radius, b - a, Vector3.Distance(b, a),1<<8);
        return r;
    }
}
