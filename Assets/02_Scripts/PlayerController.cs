using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed; //최대 속력 변수 
    public float jumpPower; //점프 높이 변수 선언
    private bool isJumping; //점프 유무 변수 선언
    public Rigidbody rigid;//물리이동을 위한 변수 선언 
    public float wallrunSpeed;
    private bool isWallSliding;
    private bool isWallDetected;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    
    //private bool canWallSlide;    

    //[Header("Collision info")]
    //[SerializeField] private Transform groundCheck;
    //[SerializeField] private float groundCheckRadius;
    //[SerializeField] private LayerMask whatIsGround;
    //                 private bool isGround;



    void Awake()
    {
        rigid = GetComponent<Rigidbody>();//변수 초기화
        jumpPower = 6;
        isJumping = false;
    }

    private void Update()
    {
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(0.5f * rigid.velocity.normalized.x, rigid.velocity.y);
        }
        if (Input.GetButtonDown("Horizontal"))
        {
            Input.GetAxisRaw("Horizontal");
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            Debug.Log("점프");
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
        Debug.Log("땅에닿음");
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode.Impulse);

        if (rigid.velocity.x > maxSpeed)
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * -(1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
    }

    

}
