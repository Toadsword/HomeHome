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

    [SerializeField] private Loup loup;

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

        Vector3 dir = Vector3.Normalize(new Vector3(horizontal, vertical, 0))*speed;

        horizontal = dir.x;
        vertical = dir.y;
    }

    void FixedUpdate()
    {
        if (!GetComponent<PlayerInventory>().isPicking)
            rigid.velocity = new Vector2(horizontal, vertical);
        else
            rigid.velocity *= 0;
    }

    public void SetSurBuisson(bool b)
    {
        surBuisson = b;
        if(b)
            makesNoise();
    }
    public void SetSurFeuillage(bool b)
    {
        surFeuillage = b;
        if(b)
            makesNoise();
    }
    
    void makesNoise()
    {
        loup.hearNoise(transform.position);
    }
}