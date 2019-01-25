using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class PlayerExample : MonoBehaviour
{
    [SerializeField] private float speed = 5;

    private Rigidbody2D rigid;

    private float horizontal, vertical;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = GameInput.GetAxisRaw(GameInput.AxisType.HORIZONTAL);
        float vertical = GameInput.GetAxisRaw(GameInput.AxisType.VERTICAL);

        horizontal *= speed;
        vertical *= speed;
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(horizontal, vertical);
    }
}