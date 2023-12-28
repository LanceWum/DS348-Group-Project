using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck _physicsCheck;
    private PlayerController _playerController;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        _physicsCheck = GetComponent<PhysicsCheck>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        SetAnimations();
    }

    public void SetAnimations()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround", _physicsCheck.isGround);
        anim.SetBool("isDead", _playerController.isDead);
        anim.SetBool("isAttack", _playerController.isAttack);
    }

    public void PlayerHurt()
    {
        anim.SetTrigger("hurt");
    }

    public void PlayAttack()
    {
        anim.SetTrigger("attack");
    }
}
