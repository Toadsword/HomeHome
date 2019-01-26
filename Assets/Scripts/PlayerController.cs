using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5;

    public bool surBuisson = false;
    public bool surFeuillage = false;

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
        horizontal = GameInput.GetAxisRaw(GameInput.AxisType.HORIZONTAL);
        vertical = GameInput.GetAxisRaw(GameInput.AxisType.VERTICAL);
        
        horizontal *= speed;
        vertical *= speed;
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(horizontal, vertical);
    }
}