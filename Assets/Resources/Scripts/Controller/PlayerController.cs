using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using Data;

public class PlayerController : MonoBehaviour
{
    Stats stats = new Stats();

    int playerDir = 1;
    [SerializeField]
    float slopeCheckDistance;
    float maxSlopeAngle = 50f;
    float groundCheckRadius = 1f;
    float h;
    float angle;


    [SerializeField]
    Transform groundCheck;
    LayerMask groundLayer;

    [SerializeField]
    bool isGrounded;
    bool isJumping;
    bool isOnSlope;

    Vector2 slopePerp;
    Vector2 playerVec;

    Vector2 colliderSize;

    Rigidbody2D rb;
    CapsuleCollider2D cc;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();

        colliderSize = cc.size;
    }

    void Start()
    {
        slopeCheckDistance = 1;
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
        SlopeCheck();
        CheckGround();
        MoveMent();
        Debug.Log(h);
    }

    void SlopeCheck()
    {
        //바로 밑으로 레이를 찍어 히트 지점에서의 법선벡터 구하기
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, slopeCheckDistance, groundLayer);

        if (hit)
        {
            //노말벡터 값에서 반시계 방향으로 90도 돌린 벡터 값
            slopePerp = Vector2.Perpendicular(hit.normal).normalized;
            //법선 벡터에서 벡터 업에 하면 각이 나오겠죠?
            angle = Vector2.Angle(hit.normal, Vector2.up);

            if(angle != 0)
            {
                isOnSlope = true;
            }
            else
            {
                isOnSlope = false;
            }

            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.yellow);
            Debug.DrawLine(hit.point, hit.point + slopePerp, Color.blue);
        }
    }


    void CheckGround()
    {
        groundLayer = LayerMask.GetMask("Ground");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (rb.velocity.y <= 0.0f)
        {
            isJumping = false;
        }
    }

    void MoveMent()
    {
        if (isGrounded && isOnSlope && !isJumping && angle < maxSlopeAngle) // 경사로 일때
        {
            rb.velocity = slopePerp * stats.Speed * h ; // -1f인 이유는 반시계 방향 벡터라 다시 원래로 돌린 것
        }
        else if(isGrounded && !isOnSlope && !isJumping) // 경사로 아닐 때
        {
            rb.velocity = new Vector2(h * stats.Speed, rb.velocity.y);
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

        if (h == 0)
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (h == 1 && playerDir == -1)
        {
            Flip();
        }
        else if (h == -1 && playerDir == 1)
        {
            Flip();
        }
    }
}

