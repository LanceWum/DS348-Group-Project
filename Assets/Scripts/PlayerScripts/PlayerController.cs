using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    public Vector2 inputDirection;
    public Rigidbody2D rb;
    private PhysicsCheck _physicsCheck;
    private PlayerAnimation _playerAnimation;
    public CapsuleCollider2D coll;
    
    [Header("Basic Parameters")]
    public float speed;
    public float jumpForce;
    public float hurtForce;
    public int combo;

    [Header("Physics Material")] 
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;
    
    [Header("Status")]
    public bool isDead;
    public bool isHurt;
    public bool isAttack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _physicsCheck = GetComponent<PhysicsCheck>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        inputControl = new PlayerInputControl();
        coll = GetComponent<CapsuleCollider2D>();
        inputControl.Gameplay.Jump.started += Jump;

        inputControl.Gameplay.Attack.started += PlayerAttack;
    }


    private void OnEnable()
    {
        inputControl.Enable();
    }
    
    private void OnDisable()
    {
        inputControl.Disable();
    }
    
    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        //Debug.Log(inputDirection);
        CheckState();
    }
    
    private void FixedUpdate()
    {
        if(!isHurt && !isAttack)
            Move();
    }
    
    /*
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.name);
    }
    */

    private void Move()
    {
        rb.velocity = new Vector2(speed * Time.deltaTime * inputDirection.x, rb.velocity.y);
        
        int faceDirection = (int)transform.localScale.x;
        
        if(inputDirection.x > 0)
        {
            faceDirection = 1;
        }
        else if(inputDirection.x < 0)
        {
            faceDirection = -1;
        }
        
        transform.localScale = new Vector3(faceDirection, 1, 1);
    }
    
    private void Jump(InputAction.CallbackContext obj)
    {
        if (_physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        _playerAnimation.PlayAttack();
        isAttack = true;
    }

    #region UnityEvent
    public void GetHurt(Transform attacker)
        {
            isHurt = true;
            rb.velocity = Vector2.zero;
            Vector2 direction = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
            
            rb.AddForce(direction * hurtForce, ForceMode2D.Impulse);
        }
    
        public void PlayerDead()
        {
            isDead = true;
            inputControl.Gameplay.Disable();
        }
    #endregion

    private void CheckState()
    {
        coll.sharedMaterial = _physicsCheck.isGround ? normal : wall;
    }
}
