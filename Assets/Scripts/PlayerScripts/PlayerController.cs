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
    
    [Header("Basic Parameters")]
    public float speed;
    public float jumpForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _physicsCheck = GetComponent<PhysicsCheck>();
        inputControl = new PlayerInputControl();
        inputControl.Gameplay.Jump.started += Jump;
        
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
    }
    
    private void FixedUpdate()
    {
        Move();
    }

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

}
