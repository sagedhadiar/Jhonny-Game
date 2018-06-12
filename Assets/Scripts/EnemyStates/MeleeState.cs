using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState {
    private EnemyController enemy;

    private float attackTimer;

    private float attackCoolDown = 3;

    private bool canAttack = true;

    public void Enter(EnemyController enemy) {
        this.enemy = enemy;
    }

    public void Execute() {

        Attack();

        if(enemy.InThrowRange && !enemy.InMeleeRange && enemy.tag == "EnemyAI") {
            enemy.ChangeState(new RangedState());
        }
        else if(enemy.Target == null || !enemy.InMeleeRange)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit() {
    }

    public void OnTriggerEnter(Collider2D other) {
    }

    private void Attack() {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCoolDown)
        {
            canAttack = true;
            attackTimer = 0;
        }

        if (canAttack)
        {
            canAttack = false;
            enemy.MyAnimator.SetTrigger("attack");
        }
    }
}
