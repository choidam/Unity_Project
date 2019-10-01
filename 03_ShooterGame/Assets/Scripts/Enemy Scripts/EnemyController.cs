using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {
    PATROL,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{
    
    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navAgent;
    
    private EnemyState enemy_State;

    public float walk_Speed = 0.5f;
    public float run_Speed = 4f;

    public float chase_Distance = 7f;
    private float current_Chase_Distance;
    public float attack_Distance = 1.8f;
    public float chase_After_Attack_Distance = 2f;

    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    public float patrol_For_This_Time = 15f;
    public float patrol_Timer;

    public float wait_Before_Attack = 2f;
    private float attack_Timer;

    private Transform target; // player

    public GameObject attack_Point;

    private EnemyAudio enemy_Audio; // enemy sound

    // private ScoreScript scoreScript;



    private void Awake() {
        enemy_Anim = GetComponent<EnemyAnimator>();
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

        enemy_Audio = GetComponentInChildren<EnemyAudio>();
        // scoreScript = GetComponent<ScoreScript>();
    }


    void Start()
    {
        enemy_State = EnemyState.PATROL;

        patrol_Timer = patrol_For_This_Time;
        
        attack_Timer = wait_Before_Attack; // attack right away

        // memorize value of chase distance 
        current_Chase_Distance = chase_Distance;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy_State == EnemyState.PATROL){
            Patrol(); // move slower
        }
        if(enemy_State == EnemyState.CHASE){
            Chase(); // run faster
        }
        if(enemy_State == EnemyState.ATTACK){
            Attack();
        }
    }


    void Patrol(){

        // tell nav agent that he can move 
        navAgent.isStopped = false;
        navAgent.speed = walk_Speed;

        patrol_Timer += Time.deltaTime; // add patorl time

        if(patrol_Timer > patrol_For_This_Time){
            SetNewRandomDestination();
            patrol_Timer = 0f;
        }

        // if moving? 
        if(navAgent.velocity.sqrMagnitude > 0) {
            enemy_Anim.Walk(true);
        } else {
            enemy_Anim.Walk(false);
        }

        // player - enemy distance
        if(Vector3.Distance(transform.position, target.position) <= chase_Distance){

            enemy_Anim.Walk(false);
            enemy_State = EnemyState.CHASE;

            // plat spotted audio
            enemy_Audio.Play_ScreamSound();

        }
    } // patrol

    void Chase(){

        // enable the agent to move again
        navAgent.isStopped = false;
        navAgent.speed = run_Speed;

        // go to player
        navAgent.SetDestination(target.position);

        if(navAgent.velocity.sqrMagnitude > 0) {
            enemy_Anim.Run(true);
        } else {
            enemy_Anim.Run(false);
        }

        // distance between enemy - player => attack!
        if(Vector3.Distance(transform.position, target.position) <= attack_Distance){

            // stop animation
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.ATTACK;

            // reset chase distance 
            if(chase_Distance != current_Chase_Distance){
                chase_Distance = current_Chase_Distance;
            }

        } else if(Vector3.Distance(transform.position, target.position) > chase_Distance) {
            
            // stop running

            enemy_Anim.Run(false);
            enemy_State = EnemyState.PATROL;

            // reset patrol timer
            patrol_Timer = patrol_For_This_Time;
            if(chase_Distance != current_Chase_Distance){
                chase_Distance = current_Chase_Distance;
            }





        } // else 

    } // chase 

    void Attack() {

        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true; // completely stop animating

        attack_Timer += Time.deltaTime;

        if(attack_Timer > wait_Before_Attack){

            enemy_Anim.Attack();

            // reset attack timer 
            attack_Timer = 0f;

            // play attack sound
            enemy_Audio.Play_AttackSound();

        }


       if(Vector3.Distance(transform.position, target.position) > attack_Distance + chase_After_Attack_Distance){
           
           enemy_State = EnemyState.CHASE;
       }



    } // attack

    void SetNewRandomDestination(){

        // 20 ~ 60
        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);

        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;

        NavMeshHit navHit; // store position 
        
        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1); 

        navAgent.SetDestination(navHit.position);

    }


    void Turn_On_AttackPoint(){
        attack_Point.SetActive(true);
    }

    void Turn_Off_AttackPoint(){
        if(attack_Point.activeInHierarchy){
            attack_Point.SetActive(false);
            // ScoreScript.score += 10;
        }
    }

    public EnemyState Enemy_State {
        get; set;
    }
}
