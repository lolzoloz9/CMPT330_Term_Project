﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class inherits from Elf and is used to create a specific class of elf.
/// In this case, an archer.
/// </summary>
/// 
/// Author: Evan Funnell    EVF
/// 
public class ElfArcher : Elf {

    public float arrowVelocity;
    public float attackDelay;
    public float arrowLifeDelay;
    public ElfArrow arrowPrefab;

    private Vector3 enemyDirection;
    private float timeSinceAttack;

    protected override void Start() {
        //arrows ignore elf
        //Physics2D.queriesStartInColliders = false;

        base.Start();


    }

    /// <summary>
    /// Update function inherits from base class update, and adds a new state check to perform actions 
    /// needed to attack the player
    /// </summary>
    /// 
    /// 2018-12-05  EVF     Initial State
    /// 2018-12-08  EPM     Added Arm Animator
    /// 
    protected override void Update() {
        base.Update();
        if (state == State.ATTACKING) {

            mainAnimator.SetBool("isAttacking", true);
            armAnimator.SetBool("isAttacking", true);
            AttackMode();
        }
        else {
            armAnimator.transform.rotation = Quaternion.Euler(0, 0, 0);
            mainAnimator.SetBool("isAttacking", false);
            armAnimator.SetBool("isAttacking", false);
        }

        timeSinceAttack += Time.deltaTime;
    }

    /// <summary>
    /// Fires an arrow at the player
    /// </summary>
    /// 
    /// 2018-12-08  EPM     Added Animations and arrow shooting
    /// 
    private void AttackMode() {
        Vector3 delta = playerPosition.position - transform.position;
        //rotate the animator to face the player
        float theta = Mathf.Atan2(delta.y, delta.x);

        enemyDirection = facingRight ? new Vector3(0, 0, theta * Mathf.Rad2Deg + 90) : new Vector3(0, 0, (theta * Mathf.Rad2Deg) - 90);

        armAnimator.transform.localRotation = Quaternion.Euler(enemyDirection);


        if (timeSinceAttack >= attackDelay) Attack();

    }

    private void Attack() {
        timeSinceAttack = 0;

        ElfArrow arrow = Instantiate(arrowPrefab, armAnimator.transform, false);

        arrow.arrowLifeDelay = arrowLifeDelay;

        arrow.transform.parent = null;

        Rigidbody2D arrowRB = arrow.GetComponent<Rigidbody2D>();

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), arrow.GetComponent<Collider2D>());

        arrowRB.velocity = facingRight ? new Vector2(arrowVelocity, 0) : new Vector2(-arrowVelocity,0);
    }
}
