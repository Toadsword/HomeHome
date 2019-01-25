using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExample : MonoBehaviour
{
    private Rigidbody2D rigid;
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

        horizontal *= 5;
        vertical *= 5;

        rigid.velocity = new Vector2(horizontal, vertical);
    }
}
