using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using Data;

public class PlayerController : MonoBehaviour
{
    Stats stats = new Stats();

    int playerDir = 1;

    float maxSlopeAngle = 50f;
    float slopeDownAngle;
    float slopeSideAngle;
    float lastSlopeAngle;
    float groundCheckRadius;
    float h;


    [SerializeField]
    Transform groundCheck;
    LayerMask groundLayer;

    [SerializeField]
    bool isGrounded;
    bool isJumping;
    bool isOnSlope;
    bool canWalkOnSlope;
    bool canJump;

    Vector2 playerVec;

    Vector2 colliderSize;

    Rigidbody2D rb;
    CapsuleCollider2D cc;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();

        colliderSize = cc.size;
        groundLayer = LayerMask.GetMask("Ground");
    }

    void Start()
    {
        // Input 매니저를 통해 어떠한 키가 눌리면 OnKeyboard 함수가 실행하도록 함
        // 혹시라도 두번 눌릴경우 취소하고 다시 하도록 함
        GameManager.Inputs.KeyAction -= OnKeyboard;
        GameManager.Inputs.KeyAction += OnKeyboard;
    }


    void Update()
    {
        OnKeyboard();
    }

    private void FixedUpdate()
    {
        CheckGround();
        MoveMent();
        Debug.Log(h);
    }

    void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0, colliderSize.y / 2);
    }

    void SlopeCheckHorizontal()
    {

    }

    void SlopeCheckVertical() 
    { 
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius);

        if (rb.velocity.y <= 0.0f)
        {
            isJumping = false;
        }

        if (isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
        {
            canJump = true;
        }
    }

    void MoveMent()
    {
        if (isGrounded && !isOnSlope && !isJumping) //if not on slope
        {
            playerVec.Set(stats.Speed * h, 0.0f);
            rb.velocity = playerVec;
        }
    }

    private void Flip()
    {
        playerDir *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }


    void OnKeyboard()
    {
        h = Input.GetAxisRaw("Horizontal");

        if(h == 1 && playerDir == -1)
        {
            Flip();
        }
        else if( h == -1 && playerDir == 1)
        {
            Flip();
        }
    }
}

