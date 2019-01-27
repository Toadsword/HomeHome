using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private PlayerInventory playerInventory;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInventory.isPicking)
            return;

        float horizontal = GameInput.GetAxisRaw(GameInput.AxisType.HORIZONTAL);
        float vertical = GameInput.GetAxisRaw(GameInput.AxisType.VERTICAL);

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        animator.SetBool("VerticalIsBigger", Mathf.Abs(horizontal) < Mathf.Abs(vertical));
        animator.speed = Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical));
    }
}
