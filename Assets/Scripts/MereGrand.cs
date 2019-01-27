using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MereGrand : MonoBehaviour
{
    float timer_animation = 0;
    float duree_animation = 1.0f;//sec
    int sprite_en_cours = 0;

    [SerializeField] Sprite grandMere1;
    [SerializeField] Sprite grandMere2;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer_animation += Time.deltaTime;
        if (timer_animation >= duree_animation) {
            timer_animation -= duree_animation;
            sprite_en_cours = (sprite_en_cours + 1) % 2;

            if (sprite_en_cours == 0)
                sprite.sprite = grandMere1;
            if (sprite_en_cours == 1)
                sprite.sprite = grandMere2;
        }
    }
}
