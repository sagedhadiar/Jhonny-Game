using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private EnemyController enemy;

    private float patrolTimer;

    private float patrolDuration = 10f;

    public void Enter(EnemyController enemy) {
        this.enemy = enemy;
    }

    public void Execute() {
        Patrol();

        enemy.Move();

        //If the enemy have a target and inThrowRange then throw else keep Patrolling
        if(enemy.Target != null && enemy.InThrowRange) { 
            enemy.ChangeState(new RangedState());
        }
    }

    public void Exit() {
    }

    public void OnTriggerEnter(Collider2D other) {
        if (other.tag == "Edge") {
            enemy.ChangeDirection();
        }
    }

    private void Patrol() {

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }

}
