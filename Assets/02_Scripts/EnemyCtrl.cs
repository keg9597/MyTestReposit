using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public enum ENEMYSTATE
    {
        NONE = -1,
        IDLE = 0,
        MOVE,
        ATTACK,
        DAMAGE,
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

    public int hp = 50;
    public int damage = 5;

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
                    enemyState = ENEMYSTATE.MOVE;
                }
                break;
            case ENEMYSTATE.MOVE:
                //float distance = (target.position - transform.position).magnitude;
                enemyAnim.SetInteger("ENEMYSTATE", (int)enemyState);
                float distance = Vector3.Distance(target.position, transform.position);
                if (distance < attackRange)
                {
                    stateTime = 0;
                    enemyState = ENEMYSTATE.ATTACK;
                }
                else
                {
                    Vector3 dir = target.position - transform.position;
                    dir.y = 0;
                    dir.Normalize();
                    enemyCharacterController.SimpleMove(dir * speed);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);
                }

                break;
            case ENEMYSTATE.ATTACK:
                enemyAnim.SetInteger("ENEMYSTATE", (int)enemyState);
                stateTime += Time.deltaTime;
                if (stateTime > attackStateMaxTime)
                {
                    stateTime = 0;
                    // 플레이어 공격
                    target.GetComponent<PlayerMovement>().DamageByEnemy();

                }
                float dist = Vector3.Distance(target.position, transform.position);
                if (dist > attackRange)
                {
                    enemyState = ENEMYSTATE.MOVE;
                    stateTime = 0;
                }
                break;
            case ENEMYSTATE.DAMAGE:
                enemyAnim.SetInteger("ENEMYSTATE", (int)enemyState);
                stateTime += Time.deltaTime;
                if (stateTime > 1.042f)
                {
                    stateTime = 0;
                    enemyState = ENEMYSTATE.MOVE;
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
        hp = 50;
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
            enemyState = ENEMYSTATE.DAMAGE;
        }
    }
}
