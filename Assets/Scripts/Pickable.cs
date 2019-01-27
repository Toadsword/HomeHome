using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Pickable : MonoBehaviour
{
    public enum PickableType
    {
        BAIE,
        BRINDILLE,
        CHAMPIGNON,
        CAILLOU,
        DOOR
    }

    float timer_animation = 0;//sec
    float duree_animation = 0.3f;//sec
    Vector3 direction_animation;
    Vector3 acceleration = new Vector3(0,-9.81f,0);

    public void lancerAnimation(Vector3 direction) {
        timer_animation = duree_animation;
        direction_animation = direction;
    }

    void Update() {
        if (timer_animation > 0) {
            timer_animation -= Time.deltaTime;

            transform.position += direction_animation * Time.deltaTime;
            direction_animation += acceleration*Time.deltaTime;
        }
    }

    [SerializeField] public PickableType typePickable;
}
