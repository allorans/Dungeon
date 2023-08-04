using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using EventArgs;
using Behaviors.LichBoss.States;

namespace Behaviors.LichBoss{

public class LichBossController : MonoBehaviour
{
    [HideInInspector] public LichBossHelper helper;
    // // Start is called before the first frame update
    
    
    // 
    [HideInInspector] public NavMeshAgent thisAgent;
    [HideInInspector] public Animator thisAnimator;

    [HideInInspector] public LifeScript thislife;

    [HideInInspector] public StateMachine stateMachine;
 
    [HideInInspector] public Idle idleState;

     [HideInInspector] public Follow followState;

     [HideInInspector] public Hurt hurtState;

     [HideInInspector] public Dead deadState;

    [HideInInspector] public AttackNormal attackNormalState;

    [HideInInspector] public AttackSuper attackSuperState;

    [HideInInspector] public AttackRitual attackRitualState;





    [Header("General:")]

    public Transform staffTop;
    public Transform staffBottom;

    public float lowHealthThreshold=0.4f;
    
    [Header("Idle:")]
    public float idleDuration = 0.3f;
    
    [Header("Follow:")]
    
    public float ceaseFollowInterval = 4f;

    
    [Header("Attack:")]
    
    public int attackDamage = 1;
    public Vector3 aimOffset=new Vector3(0,1.4f,0);


    [Header("Attack Normal:")]

    
    public float attackNormalMagicDelay = 0f;

    public float attackNormalDuration = 0f;

    public float attackNormalImpulse=10;
  
    // public float attackRadius = 1.5f;
    // public float attackSphereRadius = 1.5f;
    // public float damageDelay = 0f;
    // public float attackDuration = 1f;



    [Header("Attack Super:")]

    public float attackSuperMagicDelay = 0f;
    public float attackSuperMagicDuration = 1f;
    public int attackSuperMagicCount = 5;

    public float attackSuperDuration = 0f;
    public float attackSuperlImpulse=10;



    [Header("Attack Ritual:")]
    public float distanceToRitual = 2.5f;
    public float attackRitualDelay = 0f;

    public float attackRitualDuration = 0f;
    

    [Header("Hurt:")]

    public float hurtDuration = 0.5f;

    //  [Header("Dead:")]

    //  public float destroyIfFar = 30f;


    [Header("Magic:")]
    public GameObject fireBallPrefab;
    public GameObject energyBallPrefab;

    public GameObject ritualPrefab;


    [Header("Debug:")]
    public string currentStateName;


    [HideInInspector] public List<IEnumerator> stateCoroutines= new();


    private void Start()
    {
        stateMachine= new StateMachine();
        idleState= new Idle(this);
        followState= new Follow(this);
        attackNormalState= new AttackNormal(this);
        attackSuperState= new AttackSuper(this);
        attackRitualState= new AttackRitual(this);
        hurtState= new Hurt(this);
        deadState=new Dead(this);
        stateMachine.ChangeState(idleState);  

        thislife.OnDamage+=OnDamage;
    }

    private void OnDamage(object sender,DamageEventArgs args){
        Debug.Log("Lich Boss recebeu "+args.damage+" de dano de "+args.attacker.name);
        stateMachine.ChangeState(hurtState);

    }

    // // Update is called once per frame
    private void Awake()
    {
    
     thisAgent=GetComponent<NavMeshAgent>();  
     helper=new LichBossHelper(this); 
     thislife=GetComponent<LifeScript>();
     thisAnimator=GetComponent<Animator>();

        
    }


    private void Update()
        {
            var bossBattleHandler=GameManager.Instance.bossBattleHandler;
            if(bossBattleHandler.IsActive()){
                stateMachine.Update();
            }
            

            currentStateName=stateMachine.currentStateName;  
            var velocityRate=thisAgent.velocity.magnitude/thisAgent.speed;
            thisAnimator.SetFloat("fVelocity",velocityRate);

            
            if(!thislife.IsDead()){
            var player=GameManager.Instance.player;
            var vecToPlayer=player.transform.position-transform.position;
            vecToPlayer.y=0;
            vecToPlayer.Normalize();
            var desiredRotation=Quaternion.LookRotation(vecToPlayer);
            var newRotation=Quaternion.LerpUnclamped(transform.rotation,desiredRotation,0.2f);
            transform.rotation=newRotation;}

        }

        private void LateUpdate()
        {
            stateMachine.LateUpdate();

        }
    
    private void FixedUpdate()
        {
            stateMachine.FixedUpdate();

        }
}


}