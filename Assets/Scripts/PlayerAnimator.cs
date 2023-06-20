using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{


    private const string IS_MOVING = "IsMoving";
    private const string IS_CARRYING = "IsCarrying";

    [SerializeField] private Animator animator;
    

    private void Update()
    {
        animator.SetBool(IS_MOVING, Player.Instance.IsMoving());
        animator.SetBool(IS_CARRYING, Player.Instance.IsCarrying());
    }

}
