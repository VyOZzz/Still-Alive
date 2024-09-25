using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SwordAttack : CombatAction
{
    [SerializeField] private float KBForce = 5f;
    [SerializeField]private new int _damage;
    private Animator animator;
    [SerializeField] private DetectionZone detectionZone;
    public bool isEnemy;
    private bool isAttack = false;
    void Start()
    {
        SetDamage(_damage);
        cooldownTime = 0.5f;
        if(!isEnemy)
            animator = GetComponentInParent<Animator>();
        else if (isEnemy)
        {
            animator = GetComponentInParent<Animator>();
            detectionZone = GetComponentInChildren<DetectionZone>();
        }
    }
    public override void Attack()
    {
        if (canAttack)
        {
            isAttack = true;
            // animation attack
            animator.SetTrigger(AnimationStrings.attackTrigger);
            //cooldown
            StartCoroutine(AttackCooldown());
        }
    }
    private void  OnTriggerEnter2D(Collider2D other)
    {
        if(isAttack){
            if (!isEnemy && other.CompareTag("Enemy"))
            {
                HandleAttackOnEnemy(other);
            }
            else if (isEnemy && other.CompareTag("Player"))
            {
                HandleAttackOnPlayer(other);
            }
        }
    }
    private void HandleAttackOnEnemy(Collider2D other)
    {
        // nếu có script Enemy thì mới tấn công được
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            Debug.Log("take damage");
            enemy.TakeDamage(_damage);
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                // Calculate knockback direction
                Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                knockbackDirection.y = 1;
                // Apply knockback force
                enemyRb.AddForce(knockbackDirection * KBForce, ForceMode2D.Impulse);
            }
        }
    }
    private void HandleAttackOnPlayer(Collider2D other)
    {
        var player = other.gameObject.GetComponent<PlayerCtrl>();
        if (player != null)
        {
            player.HealthControl.TakeDamage(_damage);
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calculate knockback direction
                Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                knockbackDirection.y = 1;
                // Apply knockback force
                rb.AddForce(knockbackDirection * KBForce, ForceMode2D.Impulse);
            }
        }

        isAttack = false;
    }
}
