using System;
using UnityEngine;
using UnityEngine.AI;
using Behaviors.MeleeCreature.States;
using EventArgs;

public class MeleeCreatureController : MonoBehaviour
{
    // Start is called before the first frame update
    
    
    [HideInInspector] public MeleeCreatureHelper helper;
    [HideInInspector] public NavMeshAgent thisAgent;
    [HideInInspector] public Animator thisAnimator;

    [HideInInspector] public LifeScript thislife;

    [HideInInspector] public Collider thisCollider;

    [HideInInspector] public Rigidbody thisRigidbody;

    [HideInInspector] public StateMachine stateMachine;
 
    [HideInInspector] public Idle idleState;

     [HideInInspector] public Follow followState;

     [HideInInspector] public Hurt hurtState;

     [HideInInspector] public Dead deadState;

    [HideInInspector] public Attack attackState;




    [Header("General:")]

    public float searchRadius = 5f;

    [Header("Idle:")]
    public float targetSearchInterval = 1f;
    
    [Header("Follow:")]
    
    public float ceaseFollowInterval = 4f;

    [Header("Attack:")]

    public float distanceToAttack = 1f;
    public float attackRadius = 1.5f;

    public float attackSphereRadius = 1.5f;
    public float damageDelay = 0f;
    public float attackDuration = 1f;
    public int attackDamage = 1;

    [Header("Hurt:")]

    public float hurtDuration = 1f;

     [Header("Dead:")]

     public float destroyIfFar = 30f;


     [Header("Effects:")]

     public GameObject knockoutEffect;

     [Header("Sound:")]
     public GameObject slimeAudioSource;


    [Header("Debug:")]
    public string currentStateName;

    private void Start()
    {
        stateMachine= new StateMachine();
        idleState= new Idle(this);
        followState= new Follow(this);
        attackState= new Attack(this);
        hurtState= new Hurt(this);
        deadState=new Dead(this);
        stateMachine.ChangeState(idleState);  

        thislife.OnDamage+=OnDamage;
    }

    private void OnDamage(object sender,DamageEventArgs args){
        Debug.Log("Criatura recebeu "+args.damage+" de dano de "+args.attacker.name);
        stateMachine.ChangeState(hurtState);

    }

    // Update is called once per frame
    private void Awake()
    {
    
     thisAgent=GetComponent<NavMeshAgent>();  
     helper=new MeleeCreatureHelper(this); 
     thislife=GetComponent<LifeScript>();
     thisAnimator=GetComponent<Animator>();
     thisCollider=GetComponent<Collider>();
     thisRigidbody=GetComponent<Rigidbody>();

        
    }


    private void Update()
        {
            stateMachine.Update();

            currentStateName=stateMachine.currentStateName;  
            var velocityRate=thisAgent.velocity.magnitude/thisAgent.speed;
            thisAnimator.SetFloat("fVelocity",velocityRate);

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
