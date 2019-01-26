using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5;

    public bool surBuisson = false;
    public bool surFeuillage = false;

    private float surBuissonTimer = 0.8f;
    private float surBuissonInterval = 0.8f;

    private float surFeuillageTimer = 0.8f;
    private float surFeuillageInterval = 0.8f;

    private float walkTimer = 0.7f;
    private float walkInterval = 0.7f;

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

        if ((horizontal > 0 || vertical > 0) && GetComponent<PlayerInventory>().isPicking)
        {

            walkTimer -= Time.deltaTime;
            if (walkTimer < 0f)
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundList.PAS);
                walkTimer = walkInterval;
            }

            if (surBuisson)
            {
                surBuissonTimer -= Time.deltaTime;
                if (surBuissonTimer < 0f)
                {
                    SoundManager.Instance.PlaySound(SoundManager.SoundList.BUISSON);
                    surBuissonTimer = surBuissonInterval;
                }
            }

            if (surFeuillage)
            {
                surFeuillageTimer -= Time.deltaTime;
                if (surFeuillageTimer < 0f)
                {
                    SoundManager.Instance.PlaySound(SoundManager.SoundList.FEUILLAGE);
                    surFeuillageTimer = surFeuillageInterval;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!GetComponent<PlayerInventory>().isPicking)
            rigid.velocity = new Vector2(horizontal, vertical);
        else
            rigid.velocity *= 0;
    }
}