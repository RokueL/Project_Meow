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
        // Input �Ŵ����� ���� ��� Ű�� ������ OnKeyboard �Լ��� �����ϵ��� ��
        // Ȥ�ö� �ι� ������� ����ϰ� �ٽ� �ϵ��� ��
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
        //�ٷ� ������ ���̸� ��� ��Ʈ ���������� �������� ���ϱ�
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, slopeCheckDistance, groundLayer);

        if (hit)
        {
            //�븻���� ������ �ݽð� �������� 90�� ���� ���� ��
            slopePerp = Vector2.Perpendicular(hit.normal).normalized;
            //���� ���Ϳ��� ���� ���� �ϸ� ���� ��������?
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
        if (isGrounded && isOnSlope && !isJumping && angle < maxSlopeAngle) // ���� �϶�
        {
            rb.velocity = slopePerp * stats.Speed * h ; // -1f�� ������ �ݽð� ���� ���Ͷ� �ٽ� ������ ���� ��
        }
        else if(isGrounded && !isOnSlope && !isJumping) // ���� �ƴ� ��
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

