using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loup : MonoBehaviour
{
    public bool surBuisson = false;
    public bool surFeuillage = false;

	private Rigidbody2D rigid;
    [SerializeField]
    private GameObject centre_loup;
    
    //distance of the new target point from the current wolf position
    [SerializeField]
    private int time = 100;

    //speed of the wolf
    [SerializeField] private float walk_speed = 2.0f;
    [SerializeField] private float[] walk_speed_lvl = { 2.0f, 2.5f, 2.8f };
    [SerializeField] private float run_speed = 5.0f;
    [SerializeField] private float[] run_speed_lvl = { 3.5f, 3.8f, 4.2f };

    float current_speed = 0.0f;

    [SerializeField]
    private GameObject sweet;

    //distances and state
    float[] distances = {30.0f, 20.0f, 15.0f, 10.0f};
    float distance_too_far = 50.0f;
    private int state;

    //noise pos
    float[] noise_precisions = { 0.5f*Mathf.PI, 0.4f * Mathf.PI, 0.3f * Mathf.PI, 0.2f*Mathf.PI, 0.0f};
    Vector3 last_noise_pos = new Vector3(0.0f, 0.0f, 0.0f);
    float noise_precision = 0.5f*Mathf.PI;

    //house
    float x1_house = -5.5f;
    float x2_house = 5.5f;
    float y1_house = 0.0f;
    float y2_house = 10.0f;
   

    //timer
    private int timer = 0;

    //current direction of the wolf
    private Vector3 direction;

    //Destruction Arbre
    float timer_destruction_arbre=0;//sec
    float duree_destruction_arbre = 1;//sec
    bool enDestructionArbre=false;
    GameObject arbreEnCoursDestruction = null;
    [SerializeField]
    Pickable brindillePrefab;

    float timer_griffure = 0;
    int nb_griffures = 2;
    float timer_tremblement_arbre = 0;
    float duree_tremblement_arbre = 0.2f;
    Vector3 posArbreInitiale = new Vector3(0,0,0);

    // Start is called before the first frame update
    void Start()
    {
		rigid = GetComponent<Rigidbody2D>();
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(rigid.velocity.x<0)
            transform.localScale =new Vector3(-1.6f, 1.6f, 1.6f);
        if (rigid.velocity.x>0)
            transform.localScale =new Vector3(1.6f, 1.6f, 1.6f);

        if (enDestructionArbre) {
            gestionDestructionArbre();
        } else {
            --timer;

            UpdateState();

            //the wolf attack
            if (state == 3)
                RunToGirl();

            //the wolf walk randomly following last noise
            else if (state < 3) {
                if (timer <= 0) {
                    WanderAround();
                    timer = time;
                }
            }

            //teleport the wolf if too far
            float dist_sweety = Vector3.Distance(transform.position, sweet.transform.position);
            if (dist_sweety > distance_too_far) {
                teleport();
            }

            //move the wolf
            float alpha = 1.0f;
            if (surFeuillage || surBuisson)
                alpha = 0.5f;

            rigid.velocity = current_speed * direction * alpha;
        }
    }

    void teleport()
    {
        Vector3 new_pos;
        do
        {
            float theta = Random.Range(0f, 2 * Mathf.PI);
            new_pos = sweet.transform.position + distances[0] * new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
        } while (Vector3.Magnitude(new_pos) > 90);
        transform.position = new_pos;
    }
    void RunToGirl()
    {
        //first heaer noise
        hearNoise(sweet.transform.position);
        //deal with house ...

        Vector3 loup_pos = centre_loup.transform.position;

        if (CollidesHouse(loup_pos, sweet.transform.position,0.8f))
        {
            Debug.Log("houuuuu");
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
                if (!CollidesHouse(points[i], loup_pos, 1.00f))
                    from_wolf.Add(i);
                if (!CollidesHouse(points[i],sweet.transform.position,0.4f))
                    from_sweet.Add(i);
            }

            float best_dist = infty_;
            int best_point = 0;

            for (int i=0; i<from_wolf.Count; ++i)
            {
                for(int j = 0; j<from_sweet.Count; ++j)
                {
                    float current_dist = distance_table[from_wolf[i],from_sweet[j]]
                        + Vector3.Distance(loup_pos, points[from_wolf[i]])
                        + Vector3.Distance(sweet.transform.position, points[from_sweet[j]]);

                    if(current_dist<best_dist)
                    {
                        best_point = from_wolf[i];
                        best_dist = current_dist;
                    }
                }
            }
            direction = Vector3.Normalize(points[best_point] - loup_pos);

        }
        else
        {
            direction = Vector3.Normalize(sweet.transform.position - loup_pos);
        }

        current_speed = run_speed;
    }

    public void hearNoise(Vector3 pos)
    {
        noise_precision = noise_precisions[state];
        last_noise_pos = pos;
        SoundManager.Instance.PlaySound(SoundManager.SoundList.LOUP_GROGNEMENT);
    }

    //se balader
    void WanderAround()
    {
        Vector3 loup_pos = centre_loup.transform.position;
        int action_to_do = Random.Range(0, 5);
        //it starts walking again
        if (action_to_do != 0)
        {
            //random theta noise
            float theta_rand = Random.Range(-noise_precision*180.0f/Mathf.PI, noise_precision*180.0f/Mathf.PI);
            //initial angle with position
            direction = Quaternion.Euler(0, 0, theta_rand) * Vector3.Normalize(last_noise_pos - centre_loup.transform.position);
            current_speed =  walk_speed;
        }
        else if(action_to_do==0)
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

    bool CollidesHouse(Vector3 a, Vector3 b, float radius = 0.95f)
    {
        RaycastHit2D r = Physics2D.CircleCast(a, radius, b - a, Vector3.Distance(b, a),1<<8);
        return r;
    }

    void gestionDestructionArbre() {
        //cette partie gère la destruction d'un arbre par le loup
        timer_destruction_arbre += Time.deltaTime;


        if (timer_destruction_arbre >= duree_destruction_arbre) {
            enDestructionArbre = false;
            //supprimer/remplacer arbre
            if (arbreEnCoursDestruction != null) {
                int nb_brindilles = (int)(Random.value * 3 + 1);
                for (int i = 0; i < nb_brindilles; ++i) {
                    Pickable brind = GameObject.Instantiate(brindillePrefab);
                    float angle_rad = 2 * Mathf.PI / (float)nb_brindilles * (float)i;
                    float rayon = 3;
                    Vector3 deplacement = new Vector3(Mathf.Cos(angle_rad), Mathf.Sin(angle_rad), 0) * rayon;
                    brind.transform.position = arbreEnCoursDestruction.transform.position;
                    brind.GetComponent<Pickable>().lancerAnimation(deplacement);
                }
                Destroy(arbreEnCoursDestruction);
                arbreEnCoursDestruction = null;
            }
        }


        timer_griffure += Time.deltaTime;

        if (timer_griffure > duree_destruction_arbre / nb_griffures) {
            timer_griffure -= duree_destruction_arbre / nb_griffures;
            //lancer tremblement arbre
            timer_tremblement_arbre = duree_tremblement_arbre;
            //SoundManager.Instance.PlaySound(SoundManager.SoundList.GRAB);
            //SoundManager.Instance.PlaySound(SoundManager.SoundList.CLIC);
        }

        if (arbreEnCoursDestruction != null) {
            if (timer_tremblement_arbre > 0) {
                timer_tremblement_arbre -= Time.deltaTime;
                arbreEnCoursDestruction.transform.position = posArbreInitiale + new Vector3(0.1f * Mathf.Cos(timer_tremblement_arbre / duree_tremblement_arbre * 2 * Mathf.PI * 2), 0, 0);
            } else {
                timer_tremblement_arbre = 0;
                arbreEnCoursDestruction.transform.position = posArbreInitiale;
            }
        }
        rigid.velocity = current_speed * direction * 0;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Arbre" && other.gameObject!=arbreEnCoursDestruction) {
            enDestructionArbre = true;
            timer_destruction_arbre = 0;
            timer_griffure = 0;
            arbreEnCoursDestruction = other.gameObject;
            posArbreInitiale = arbreEnCoursDestruction.transform.position;
        }
        if (other.gameObject.tag == "Sweet")
        {
            gameOver();
        }
    }

    public void SetSurBuisson(bool b)
    {
        surBuisson = b;    
        SoundManager.Instance.PlaySound(SoundManager.SoundList.BUISSON);
    }
    public void SetSurFeuillage(bool b)
    {
        surFeuillage = b;
        SoundManager.Instance.PlaySound(SoundManager.SoundList.FEUILLAGE);
    }

    public void LeveluUp(int newLevel)
    {
        run_speed = run_speed_lvl[newLevel];
        walk_speed = walk_speed_lvl[newLevel];
    }

    void gameOver()
    {
        SceneManagement.Instance.ChangeScene(SceneManagement.Scenes.END_GAME);
    }

}
