using System;
using System.Collections;
using System.Collections.Generic;
using EventArgs;
using Player.States;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
 
    [HideInInspector] public StateMachine stateMachine;
 
    [HideInInspector] public Idle idleState;

     [HideInInspector] public Walking walkingState;

     [HideInInspector] public Jump jumpState;

     [HideInInspector] public Dead deadState;

    [HideInInspector] public Attack attackState;

    [HideInInspector] public Defend defendState;

    [HideInInspector] public Hurt hurtState;

    [HideInInspector] public Collider thisCollider;

    [HideInInspector] public Rigidbody thisRigidBody;

    [HideInInspector] public Animator thisAnimator;

    [HideInInspector] public LifeScript thisLife;


    [Header("Movement")]

    [HideInInspector] public Vector2 movementVector;
    public float movementSpeed=10;
    public float maxSpeed=10;


    [Header("Footsteps")]
    public List<AudioClip> footstepSounds;
    public AudioSource footstepAudioSource;
    public float footstepInterval = 0.33f;


    [Header("Jump")]
    [HideInInspector] public bool hasJumpInput;

    public float jumpPower=10;

    public float jumpMovementFactor = 1f;



    [Header("Slope")]
    [HideInInspector] public bool isOnSlope;

     public float maxSlopeAngle=45f;

     [HideInInspector] public Vector3 slopeNormal;

     [HideInInspector] public bool isGrounded;


    [Header("Attack")]
    public int attackStages;
    public List<float> attackStageDuration;
    public List<float> attackStageMaxIntervals;
    public List<float> attackStageInpulses;
    public GameObject swordHitBox;

    public float swordKnockbackImpulse=10;

    public List<int> damageState;


    [Header("Defend")]

    public GameObject shieldHitBox;
    public float shieldKnockbackImpulse=10f;
    [HideInInspector] public bool hasDefenseInput;


    [Header("Hurt")]

    public float hurtDuration=0.2f;



    [Header("Effects")]

        public GameObject hitEffect;


   void Awake() {
     thisRigidBody=GetComponent<Rigidbody>();
     thisAnimator=GetComponent<Animator>(); 
     thisCollider=GetComponent<Collider>();
    

     thisLife=GetComponent<LifeScript>();
     if(thisLife!=null){
        thisLife.OnDamage+=OnDamage;
        thisLife.OnHeal+=OnHeal;
        // thisLife.OnHeal+=OnHeal;
        thisLife.canInflictDamageDelegate +=CanInflictDamage;
     }

    }

    
        // Start is called before the first frame update
    void Start()
    {
        stateMachine= new StateMachine();
        idleState= new Idle(this);
        walkingState= new Walking(this);
        attackState= new Attack(this);
        defendState= new Defend(this);
        hurtState= new Hurt(this);
        jumpState=new Jump(this);
        deadState=new Dead(this);
        stateMachine.ChangeState(idleState);

        swordHitBox.SetActive(false);
        shieldHitBox.SetActive(false);

        var gameplayUI=GameManager.Instance.gameplayUI;
        gameplayUI.playerHealthBar.SetMaxHealth(thisLife.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {

          


       bool isUp = Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow);
       bool isLeft = Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.LeftArrow);
       bool isRight = Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.RightArrow);
       bool isDown = Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.DownArrow);
    
       float inputX = isRight ? 1 : isLeft ? -1 : 0;
       float inputY = isUp ? 1 : isDown ? -1 : 0;
       
       movementVector = new Vector2(inputX,inputY);
       
       hasJumpInput=Input.GetKey(KeyCode.Space);

       hasDefenseInput=Input.GetMouseButton(1);
       
       float velocity=thisRigidBody.velocity.magnitude;

       float velocityRate = velocity/maxSpeed;

       thisAnimator.SetFloat("fVelocity", velocityRate);

          

       DetectGround();
       DetectSlope();

     
       
    var bossBattleHandler=GameManager.Instance.bossBattleHandler;
    var isInCutscene=bossBattleHandler.IsInCutscene();
    if(isInCutscene&&stateMachine.currentStateName!=idleState.name){
        stateMachine.ChangeState(idleState);
    }
    
       stateMachine.Update();
        
    }

    void LateUpdate()
        {
        stateMachine.LateUpdate();

        }
    
    void FixedUpdate()
        {
        Vector3 gravityForce=Physics.gravity*(isOnSlope?0.50f:1f);
        thisRigidBody.AddForce(gravityForce, ForceMode.Acceleration);

        LimitSpeed();
        stateMachine.FixedUpdate();
        }


    private void OnDamage(object sender, DamageEventArgs args){
        var gameplayUI=GameManager.Instance.gameplayUI;
        gameplayUI.playerHealthBar.SetHealth(thisLife.health);
         if(GameManager.Instance.isGameOver) return;

        Debug.Log("Player recebeu um dano de " + args.damage + " do "+args.attacker.name);
        stateMachine.ChangeState(hurtState);

    }

     private void OnHeal(object sender, HealEventArgs args){
        var gameplayUI=GameManager.Instance.gameplayUI;
        gameplayUI.playerHealthBar.SetHealth(thisLife.health);

        Debug.Log("Player recebeu cura."); 

    }

    private bool CanInflictDamage(GameObject attacker,int damage){
        

        var isDefending=stateMachine.currentStateName==defendState.name;
        if(isDefending){
        Vector3 playerDirection= transform.TransformDirection(Vector3.forward);
        Vector3 attackDirection=(transform.position-attacker.transform.position).normalized;
        float dot=Vector3.Dot(playerDirection,attackDirection);
        if(dot< -0.25){
                return false;
        }
        
        }
       
        return true;
    }


    public Quaternion GetForward(){
         Camera camera=Camera.main;
         float eulerY = camera.transform.eulerAngles.y;
         return Quaternion.Euler(0,eulerY,0);
    }



public void OnShieldCollisionEnter(Collider other){
        var otherObject = other.gameObject;
        var otherRigidbody = otherObject.GetComponent<Rigidbody>();
        var isTarget = otherObject.layer == LayerMask.GetMask("Target","Creatures");
        if(isTarget && otherRigidbody != null){
            var positionDiff = otherObject.transform.position - gameObject.transform.position;
            var impulseVector = new Vector3(positionDiff.normalized.x,0,positionDiff.normalized.z);
            impulseVector *= shieldKnockbackImpulse;
            otherRigidbody.AddForce(impulseVector,ForceMode.Impulse);
        }
    }




    public void OnSwordCollisionEnter(Collider other){
        var otherObject = other.gameObject;
        var otherRigidbody = otherObject.GetComponent<Rigidbody>();
        var otherCollider=otherObject.GetComponent<Collider>();
         var otherlife = otherObject.GetComponent<LifeScript>();
        
        int bit = 1 << otherObject.layer;
        int mask= LayerMask.GetMask("Target","Creatures");
        bool isBitInMask=(bit & mask) == bit;
        bool isTarget=isBitInMask;
        


        if(isTarget && otherRigidbody != null){


         if(otherlife!=null){
            var damage=damageState[attackState.stage-1];
            otherlife.InflictDamage(gameObject,damage);
        }



            if(otherRigidbody!=null){
            var positionDiff = otherObject.transform.position - gameObject.transform.position;
            var impulseVector = new Vector3(positionDiff.normalized.x,0,positionDiff.normalized.z);
            impulseVector *= swordKnockbackImpulse;
            otherRigidbody.AddForce(impulseVector,ForceMode.Impulse);
        }
        
        
        if(hitEffect!=null){
            var hitPosition=otherCollider.ClosestPointOnBounds(swordHitBox.transform.position);
            var hitRotation=hitEffect.transform.rotation;
            Instantiate(hitEffect,hitPosition,hitRotation);
        }


        }
    }

    public void RotateBodyToFaceInput(float alpha = 0.225f){
        if(movementVector.IsZero()) return;
        Camera camera=Camera.main;
        Vector3 inputVector= new Vector3(movementVector.x,0,movementVector.y);
        Quaternion q1= Quaternion.LookRotation(inputVector,Vector3.up);
        Quaternion q2= Quaternion.Euler(0,camera.transform.eulerAngles.y,0);
        Quaternion toRotation = q1*q2;
        Quaternion newRotation = Quaternion.LerpUnclamped(transform.rotation, toRotation,alpha);

        thisRigidBody.MoveRotation(newRotation);

    }



    public bool AttemptToAttack(){

    if(Input.GetMouseButtonDown(0)){
                var isAttacking = stateMachine.currentStateName==attackState.name;
                var canAttack= !isAttacking || attackState.CanSwitchStages();
                if (canAttack){
                    var attackStage= isAttacking ? (attackState.stage+1) : 1;
                    attackState.stage = attackStage;
                    stateMachine.ChangeState(attackState);
                    return true;
                }
            }
            return false;

    }


    private void DetectGround(){

        isGrounded=false;

        Vector3 origin =transform.position;
        Vector3 direction =Vector3.down;
        Bounds bounds=thisCollider.bounds;
        float radius=bounds.size.x * 0.33f;
        float maxDistance=0.1f;
        LayerMask groundLayer= GameManager.Instance.groundLayer;
        if(Physics.Raycast(origin, direction, maxDistance,groundLayer)){
            isGrounded=true;
        }
    }


private void DetectSlope(){

        isOnSlope=false;

        slopeNormal=Vector3.zero;

        Vector3 origin =transform.position;
        Vector3 direction =Vector3.down;
       
        float maxDistance=0.2f;
        
        if(Physics.Raycast(origin, direction,out var slopeHitInfo, maxDistance)){
            float angle = Vector3.Angle(Vector3.up,slopeHitInfo.normal); 
            isOnSlope= angle<maxSlopeAngle && angle != 0;

            slopeNormal= isOnSlope?slopeHitInfo.normal:Vector3.zero;
            
        }
    }

    
private void LimitSpeed(){

    Vector3 flatVelocity=new Vector3(thisRigidBody.velocity.x,0,thisRigidBody.velocity.z);
    if(flatVelocity.magnitude>maxSpeed){
        Vector3 limitedVelocity=flatVelocity.normalized*maxSpeed;
        thisRigidBody.velocity= new Vector3(limitedVelocity.x,thisRigidBody.velocity.y,limitedVelocity.z);
    }

}

private void OnTriggerEnter(Collider other) {

if(other.CompareTag("BossRoomSensor")){
    GlobalEvents.Instance.InvokeOnBossRoomEnter(this,new BossRoomEnterArgs());
    Destroy(other.gameObject);
}

}

}
