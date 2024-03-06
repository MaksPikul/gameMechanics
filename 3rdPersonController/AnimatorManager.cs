using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    int horizontal;
    int vertical;
    

    private void Awake(){
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void PlayTargetAnimation(string targetAnimation, bool doingAction){

        animator.SetBool("doingAction", doingAction);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void UpdateAnimatorValues(float horizontalMove, float verticalMove, bool isSprinting){

        if (isSprinting){
            horizontalMove = 2;
            verticalMove = 2;
        }

        animator.SetFloat(horizontal, horizontalMove, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, verticalMove, 0.1f, Time.deltaTime);
    }
}
