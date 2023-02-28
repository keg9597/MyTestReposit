using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Animator anim;
    public Transform character;

    public float speed = 3f;
    public float jumpPower = 5f;
    public float rotateSpeed;

    public int jumpCnt = 1;

    private Rigidbody rb;
    
    public Vector3 movement;
    public float horizontalMove;
    public float verticalMove;
    public bool isJumping;
    public bool isGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        jumpCnt = 1;
        isGround = true;
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            if(jumpCnt < 2)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().AddForce(Vector3.up * 5f);
                jumpCnt++;
            }    
        }
        AnimationUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGround = true;
            jumpCnt = 1;
        }
    }

    private void FixedUpdate()
    {
        Run();
        Jump();
        Turn();
    }

    void Run()
    {
        movement.Set(horizontalMove, 0, verticalMove);
        movement = movement.normalized * speed * Time.deltaTime;

        rb.MovePosition(transform.position + movement);
    }

    void Turn()
    {
        if (horizontalMove == 0 && verticalMove == 0)
        {
            return;
        }
        Quaternion newRotation = Quaternion.LookRotation(movement);
        rb.rotation = Quaternion.Slerp (rb.rotation, newRotation, rotateSpeed * Time.deltaTime);
    }

    void Jump()
    {
        if (!isJumping)
        {
            return;
        }            
        rb.AddForce(Vector3.up * jumpPower,ForceMode.Impulse);
        
        isJumping = false;
        
        if(isGround)
        {
            jumpCnt = 1;
            if(Input.GetButtonDown("Jump"))
            {
                if(jumpCnt==1)
                {
                    rb.AddForce(0, 5, 0);
                    isGround = false;
                    jumpCnt = 0;
                }
            }
        }
    }

    void AnimationUpdate()
    {
        if(horizontalMove == 0 && verticalMove == 0)
        {
            anim.SetBool("isRun", false);
            
        }
        else
        {
            anim.SetBool("isRun", true);
            
        }
        
    }

}
