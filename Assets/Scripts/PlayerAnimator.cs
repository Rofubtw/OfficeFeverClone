using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{


    private const string IS_MOVING = "IsMoving";


    [SerializeField] private PlayerController playerController;


    private Animator _animator;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(IS_MOVING, playerController.IsMoving());
    }

}
