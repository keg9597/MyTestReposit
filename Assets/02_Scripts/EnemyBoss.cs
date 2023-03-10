using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    public enum ENEMYSTATE
    {
        NONE = -1,
        IDLE = 0,
        ATTACK1,
        ATTACK2,
        DEAD
    }
    public ENEMYSTATE enemyState;

    public float stateTime;
    public float idleStateTime;
    public Animator enemyAnim;
    public Transform target;

    public float speed = 2f;
    public float rotationSpeed = 10f;
    public float attackRange = 2.5f;
    public float attackStateMaxTime = 1f;

    public CharacterController enemyCharacterController;
    public PlayerMovement movement;

    public int hp = 1000;
    public int maxHp = 1000;
    public int damage = 10;

    private bool useAttack1 = true;

    public GameObject obj;
    public Transform firePos;
    public bool hasFired;

    void Start()
    {
        enemyState = ENEMYSTATE.IDLE;
        target = GameObject.Find("Player").transform;
        enemyCharacterController = GetComponent<CharacterController>();
        enemyAnim = GetComponentInChildren<Animator>();
        movement = target.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (movement.isDead)
        {
            enemyAnim.enabled = false;
            return;
        }

        switch (enemyState)
        {
            case ENEMYSTATE.NONE:
                //Debug.Log("아무것도 안함!!!!!");
                break;
            case ENEMYSTATE.IDLE:
                enemyAnim.SetInteger("ENEMYSTATE", (int)ENEMYSTATE.IDLE);
                stateTime += Time.deltaTime;
                if (stateTime > idleStateTime)
                {
                    stateTime = 0;
                    if (useAttack1)
                    {
                        enemyState = ENEMYSTATE.ATTACK1;
                        useAttack1 = false;
                    }
                    else
                    {
                        enemyState = ENEMYSTATE.ATTACK2;
                        useAttack1 = true;
                    }
                }
                else if (hp <= maxHp * 0.5f) // add this else if statement
                {
                    useAttack1 = false;
                }
                break;
            case ENEMYSTATE.ATTACK1:
                enemyAnim.SetInteger("ENEMYSTATE", (int)ENEMYSTATE.ATTACK1);
                stateTime += Time.deltaTime;
                if (stateTime > attackStateMaxTime)
                {
                    stateTime = 0;
                    // 플레이어 공격
                    target.GetComponent<PlayerMovement>().DamageByEnemy();
                }
                float dist1 = Vector3.Distance(target.position, transform.position);
                if (dist1 > attackRange)
                {
                    enemyState = ENEMYSTATE.IDLE;
                    stateTime = 0;
                }
                break;
            case ENEMYSTATE.ATTACK2:
                enemyAnim.SetInteger("ENEMYSTATE", (int)ENEMYSTATE.ATTACK2);
                if (!hasFired)
                {
                    Instantiate(obj, firePos.position, firePos.rotation);
                    hasFired = true;
                }
                stateTime += Time.deltaTime;
                if (stateTime > attackStateMaxTime)
                {
                    stateTime = 0;
                    // 플레이어 공격
                    target.GetComponent<PlayerMovement>().DamageByEnemy();
                }
                float dist2 = Vector3.Distance(target.position, transform.position);
                if (dist2 > attackRange)
                {
                    enemyState = ENEMYSTATE.IDLE;
                    stateTime = 0;
                    hasFired = false;
                }
                if (hp <= 0)
                {
                    enemyState = ENEMYSTATE.DEAD;
                }
                break;
            case ENEMYSTATE.DEAD:
                //enemyAnim.SetInteger("ENEMYSTATE", (int)enemyState);
                //Destroy(gameObject, 3f);
                enemyAnim.SetTrigger("DEAD");
                enemyCharacterController.enabled = false;

                AnimatorClipInfo[] animClipInfo;
                animClipInfo = enemyAnim.GetCurrentAnimatorClipInfo(0);
                //Debug.Log(animClipInfo[0].clip.length);

                StartCoroutine(DeadProcess(animClipInfo[0].clip.length));
                enemyState = ENEMYSTATE.NONE;
                break;
            default:
                break;
        }
    }

    void Fire()
    {
        // Calculate the forward direction based on the fire position's rotation
        Vector3 forwardDirection = firePos.forward;

        // Instantiate the object with the forward direction
        GameObject projectile = Instantiate(obj, firePos.position, Quaternion.LookRotation(forwardDirection));

        // Apply a force to the projectile to make it move forward
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        if (projectileRigidbody != null)
        {
            projectileRigidbody.AddForce(forwardDirection * 1000f);
        }
    }

    IEnumerator DeadProcess(float t)
    {
        yield return new WaitForSeconds(t);
        while (transform.position.y > -t)
        {
            Vector3 temp = transform.position;
            temp.y -= Time.deltaTime;
            transform.position = temp;
            yield return new WaitForEndOfFrame();
        }
        //Destroy(gameObject);
        InitEnemy();
        gameObject.SetActive(false);
    }

    void InitEnemy()
    {
        hp = 1000;
        enemyState = ENEMYSTATE.IDLE;
        enemyCharacterController.enabled = true;
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            damage = 5;
        }

        if (collision.gameObject.tag == "Bullet")
        {
            int dmg = collision.gameObject.GetComponent<BulletCtrl>().damage;
            hp -= dmg;
        }
    }
}
