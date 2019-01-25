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
        animator.SetFloat("Horizontal", GameInput.GetAxisRaw(GameInput.AxisType.HORIZONTAL));
        animator.SetFloat("Vertical", GameInput.GetAxisRaw(GameInput.AxisType.VERTICAL));

        //TODO Ajouter condition pour le pickup
    }
}
