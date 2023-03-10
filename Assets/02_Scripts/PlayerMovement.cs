using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator anim;

    public Vector3 moveVector;
    private Vector3 lastMove;
    private float speed = 8;
    private float jumpforce = 8;
    private float gravity = 25;
    private float verticalVelocity;
    private float rotateSpeed = 10;

    public int hp = 100;
    public int maxhp = 100;
    public int damage = 10;
    public bool isDead;
 
    public CharacterController controller;
    public Transform character;
    public GameObject boss;
    public GameObject obj;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -1;
            moveVector = Vector3.zero;
            moveVector.x = Input.GetAxisRaw("Horizontal");
            if (moveVector.x > 0)
            {
                anim.SetBool("isRun_1", true);
                character.localRotation = Quaternion.Euler(0, 90, 0);
            }
            else if (moveVector.x < 0)
            {
                anim.SetBool("isRun_1", true);
                character.localRotation = Quaternion.Euler(0, -90, 0);
            }
            else
            {
                anim.SetBool("isRun_1", false);
            }
            
            moveVector.z = Input.GetAxisRaw("Vertical"); 
            if (moveVector.z > 0)
            {
                anim.SetBool("isRun", true);
            }
            else if (moveVector.z < 0)
            {
                anim.SetBool("isRun", true);
            }
            else
            {
                anim.SetBool("isRun", false);
            }

            if (Input.GetButtonDown("Jump"))
            {
                anim.SetBool("isJump", true);
                anim.SetTrigger("doJump");
                verticalVelocity = jumpforce;
            }
            else
            {
                anim.SetBool("isJump", false);
            }
            if (Input.GetButtonDown("Dash"))
            {
                anim.SetTrigger("isDash");
            }

            // new code to rotate player when moving vertically
            if (moveVector.z > 0)
            {
                character.localRotation = Quaternion.Lerp(character.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * rotateSpeed);
            }
            else if (moveVector.z < 0)
            {
                character.localRotation = Quaternion.Lerp(character.localRotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * rotateSpeed);
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
            moveVector = lastMove;
        }

        moveVector.y = 0;
        moveVector.Normalize();
        moveVector *= speed;
        moveVector.y = verticalVelocity;

        controller.Move(moveVector * Time.deltaTime);
        lastMove = moveVector;

        Attack();
    }

    void Attack()
    {
        if(Input.GetButtonDown("Attack"))
        {
            anim.SetTrigger("doShot");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obj")
        {
            int dmg = collision.gameObject.GetComponent<EnemyBossobj>().damage;
            hp -= dmg;
            //hp--;
            Debug.Log("??????");
            if (hp <= 0)
            {
                Debug.Log("????????");
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Debug.Log("??????");
            hp--;
            if (hp <= 0)
            {
                Debug.Log("????????");
                Destroy(gameObject);
            }
        }
        if (collider.gameObject.tag == "Boss")
        {
            //int dmg = boss.GetComponent<EnemyBossCtrl>().damage;
            //hp -= dmg;
            hp--;
            Debug.Log("??????");
            if (hp <= 0)
            {
                Debug.Log("????????");
                Destroy(gameObject);
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(!controller.isGrounded && hit.normal.y < 0.5f)
        {           
            if (Input.GetButtonDown("Jump"))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);
                verticalVelocity = jumpforce;
                moveVector = hit.normal * speed;
                if (moveVector.x > 0)
                {
                    anim.SetBool("isRun", true);
                    character.localRotation = Quaternion.Euler(0, 90, 0);
                }
                else if (moveVector.x < 0)
                {
                    anim.SetBool("isRun", true);
                    character.localRotation = Quaternion.Euler(0, -90, 0);
                }
            }           
        }        
    }

    public void DamageByEnemy()
    {
        if (hp <= 0)
        {
            isDead = true;
        }
        if (isDead == true)
            return;
    }
}
