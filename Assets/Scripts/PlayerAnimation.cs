using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = GameInput.GetAxisRaw(GameInput.AxisType.HORIZONTAL);
        float vertical = GameInput.GetAxisRaw(GameInput.AxisType.VERTICAL);

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        animator.SetBool("VerticalIsBigger", Mathf.Abs(horizontal) < Mathf.Abs(vertical));

        animator.speed = Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical));
        //TODO Ajouter condition pour le pickup
        
    }
}
