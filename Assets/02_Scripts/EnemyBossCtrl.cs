using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossCtrl : MonoBehaviour
{
    public Animator anim;
    public CharacterController controller;
    public PlayerMovement movement;
    public GameObject enemy;
    public GameObject weapon;
    public Transform target;
    
    private Rigidbody rb;
    private bool isAttacking = false;

    public float enemyInRange;
    public float nextAttackTime;
    public float attackRate;
    public float attackTime;

    public int hp = 1000;
    public int maxHp = 1000;
    public int damage = 10;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        target = GameObject.Find("Player").transform;
        anim = GetComponentInChildren<Animator>();
        movement = target.GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;

        // Play the attack animation
        anim.SetTrigger("Attack");

        // Wait for the animation to finish
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        isAttacking = false;
    }

    private void Update()
    {
        if (hp <= 0)
        {
            anim.SetTrigger("isDEAD");
            Destroy(gameObject, 2f);
            controller.enabled = false;
            attackTime = 0;
            target.GetComponent<PlayerMovement>().DamageByEnemy();
        }
        if (enemyInRange > Time.time && enemyInRange > nextAttackTime)
        {
            if (hp == maxHp)
            {
                anim.SetTrigger("Attack1");
                // perform Attack1
            }
            else if (hp == maxHp / 2)
            {
                anim.SetTrigger("Attack2");
                // perform Attack2
            }

            // set the next attack time
            nextAttackTime = Time.time + attackRate;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            EnemyBossCtrl boss = collision.gameObject.GetComponent<EnemyBossCtrl>();
            if (boss != null)
            {
                boss.ReduceDamage(damage);
            }
        }
        if (collision.gameObject.tag == "Bullet")
        {
            int dmg = collision.gameObject.GetComponent<BulletCtrl>().damage;
            hp -= dmg;
        }
        if (collision.gameObject.tag == "Player")
        {
            damage = 10;
        }
    }

    void ReduceDamage(int reductionAmount)
    {
        damage -= reductionAmount;
    }

    void AttackPlayer()
    {
        // Instantiate the weapon game object
        GameObject weaponInstance = Instantiate(weapon, transform.position + transform.forward, Quaternion.identity);

        // Set the weapon's position and rotation
        weaponInstance.transform.parent = transform;
        weaponInstance.transform.localPosition = weapon.transform.localPosition;
        weaponInstance.transform.localRotation = weapon.transform.localRotation;
    }
}
