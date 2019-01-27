using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManagerPlayer : MonoBehaviour
{
    private Rigidbody2D rigid;
    private int decalage = 1;

    [SerializeField] private GameObject lamp_left;
    [SerializeField] private GameObject lamp_right;

    private SpriteRenderer lamp_sprite;
    private Transform lamp;
    private Animator animator;
    private Transform sprite;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        lamp = transform.GetChild(3);
        if (lamp != null)
        {
            lamp_sprite = lamp.transform.GetChild(0).GetComponent<SpriteRenderer>();
            sprite = transform.GetChild(0);
            animator = sprite.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 100);

        if (rigid.velocity.y < 0 || rigid.velocity.x <0)
            decalage = 1;
        else if (rigid.velocity.y > 0 || rigid.velocity.x < 0)
            decalage = -1;

        /*if (rigid.velocity.y < 0 || rigid.velocity.x > 0)
            lamp.transform.position = lamp_right.transform.position;
        else if (rigid.velocity.y > 0 || rigid.velocity.x < 0)
            lamp.transform.position = lamp_left.transform.position;*/

        if (lamp == null)
            return;

        AnimatorStateInfo infos_anim = animator.GetCurrentAnimatorStateInfo(0);

        if (infos_anim.IsName("Left"))
        {
            decalage = 1;
            lamp.transform.position = lamp_left.transform.position;
        }
        else if (infos_anim.IsName("Right"))
        {
            decalage = -1;
            lamp.transform.position = lamp_right.transform.position;
        }
        else if (infos_anim.IsName("Up"))
        {
            decalage = -1;
            lamp.transform.position = lamp_left.transform.position;
        }
        else if (infos_anim.IsName("Down"))
        {
            decalage = 1;
            lamp.transform.position = lamp_right.transform.position;
        }

        lamp_sprite.sortingOrder = -(int)(transform.position.y * 100) + decalage;
    }
}
