using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loup : MonoBehaviour
{
<<<<<<< HEAD
	private Rigidbody2D rigid;
    
    //distance of the new target point from the current wolf position
    [SerializeField]
    private int time = 10;

    //speed of the wolf
    [SerializeField]
    private float speed = 1.0f;

    private int timer = 0;

    //current direction of the wolf
    private Vector3 direction;
=======
    [SerializeField] private PlayerExample player;

    private Rigidbody2D rigid;
>>>>>>> d6552f46225acfda8f50909638af0c0d7f8e5d57

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
		rigid = GetComponent<Rigidbody2D>();
=======
        
>>>>>>> d6552f46225acfda8f50909638af0c0d7f8e5d57
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        --timer;
        if (timer <= 0)
        {
            UpdateState();
            timer = time;
        }
    }

    void UpdateState()
    {
        int action_to_do = Random.Range(0, 2);
        //it starts walking again
        if (action_to_do == 1)
        {
            //update direction
            float theta = Random.Range(0f, 2 * Mathf.PI);
            float delta = Random.Range(0.8f, 1.2f);
            direction = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
            rigid.velocity = speed * direction;
        }

        else if (action_to_do == 0)
        {
            //it makes a little break
            rigid.velocity = new Vector3(0, 0, 0);
        }
=======

>>>>>>> d6552f46225acfda8f50909638af0c0d7f8e5d57
    }
}
