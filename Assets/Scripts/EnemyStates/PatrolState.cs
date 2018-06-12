using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState {
    private EnemyController enemy;

    private float patrolTimer;

    private float patrolDuration ;

    public void Enter(EnemyController enemy) {

        patrolDuration = UnityEngine.Random.Range(6, 10);

        //patrolDuration = enemy.PatrolDur; 

        this.enemy = enemy;
    }

    public void Execute() {
        Patrol();

        enemy.Move();

        if (enemy.tag == "EnemyAI") {
            //If the enemy have a target and inThrowRange then throw else keep Patrolling
            if (enemy.Target != null && enemy.InThrowRange)
            {

                enemy.ChangeState(new RangedState());
            }
        }
        else
        {
            if (enemy.Target != null && enemy.InMeleeRange)
            {

                enemy.ChangeState(new MeleeState());
            }
        }
    }

    public void Exit() {
    }

    public void OnTriggerEnter(Collider2D other) {
       
        if (other.tag == "PlayerKnife") { 
            enemy.Target = PlayerController.Instance.gameObject; 
        }
    }

    private void Patrol() {
        //patrolDuration = enemy.PatrolDur;

        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolDuration) {
            enemy.ChangeState(new IdleState());
        }
    }

}
