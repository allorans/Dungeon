using System.Collections;
using System.Collections.Generic;
using Projectiles;
using UnityEngine;

namespace Behaviors.LichBoss.States{

public class AttackSuper : State
{
    private LichBossController controller;
    private LichBossHelper helper;

    private float endAttackCooldown;

    private IEnumerator attackCoroutine;

    public AttackSuper(LichBossController controller):base("AttackSuper"){
    this.controller=controller;
    this.helper= controller.helper;
    }
    
    
public override void Enter()
    {
        base.Enter();
        
        endAttackCooldown=controller.attackSuperDuration;

        controller.thisAnimator.SetTrigger("tAttackSuper");



        var delayStep=controller.attackSuperMagicDuration/(controller.attackSuperMagicCount-1);
        for(int i=0; i<controller.attackSuperMagicCount;i++){
            var delay=controller.attackSuperMagicDelay+delayStep*i;

            helper.StartStateCoroutine(
        ScheduleAttack(delay));
        }

        // for(int i=0; i<controller.attackSuperMagicCount-1;i++){
        //     var delay=controller.attackSuperMagicDelay+delayStep*i;

        //     helper.StartStateCoroutine(
        // ScheduleAttack(delay));
        // }
          
    }

    public override void Exit()
        {
            base.Exit();
            helper.ClearStateCoroutines();
            
           
            }
        

    public override void Update()
        {
            base.Update();
             if((endAttackCooldown-=Time.deltaTime)<=0f){
                controller.stateMachine.ChangeState(controller.idleState);
                return;

            }
            
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

        }
    
    public override void FixedUpdate()
        {
            base.FixedUpdate();

        }

private IEnumerator ScheduleAttack(float delay){
        yield return new WaitForSeconds(delay);
        Debug.Log("Atacou com "+this.name);

        var spawnTransform=controller.staffTop;
         var projectile=Object.Instantiate(controller.energyBallPrefab,spawnTransform.position,spawnTransform.rotation);


        var projectileCollision= projectile.GetComponent<ProjectileCollision>();
        projectileCollision.attacker=controller.gameObject;
        projectileCollision.damage=controller.attackDamage;


        var projectileRigidbody=projectile.GetComponent<Rigidbody>();
        var player=GameManager.Instance.player;


        var vectorToPlayer=(player.transform.position+controller.aimOffset-spawnTransform.position).normalized;
        var forceVector=spawnTransform.rotation*Vector3.forward;
        forceVector= new Vector3 (forceVector.x,vectorToPlayer.y,forceVector.z);
        forceVector *= controller.attackSuperlImpulse;
        projectileRigidbody.AddForce(forceVector,ForceMode.Impulse);
        



        Object.Destroy(projectile,10);

    }


    // private IEnumerator ScheduleAttack(){
    //     yield return new WaitForSeconds(controller.damageDelay);
    //     PerformAttack();
    // }
    // private void PerformAttack(){
    //     Debug.Log("Performing Attack!");

    //     var origin=controller.transform.position;
    //     var direction=controller.transform.rotation*Vector3.forward;
    //     var radius=controller.attackRadius;
    //     var maxDistance=controller.attackSphereRadius;


    //     var attackPosition=direction*radius+origin;
    //     var layerMask=LayerMask.GetMask("Player");
    //     Collider[] colliders=Physics.OverlapSphere(attackPosition,maxDistance,layerMask);

    //     foreach(var collider in colliders){


    //         var hitObject=collider.gameObject;
    //         Debug.Log("Hit something: " + hitObject.name);
            

    //             var hitLifeScript=hitObject.GetComponent<LifeScript>();
    //             if(hitLifeScript!=null){
    //                 var attacker=controller.gameObject;
    //                 var damage=controller.attackDamage;

    //                 hitLifeScript.InflictDamage(attacker,damage);
    //             }

            
        
    //     }
        
    // }
}}



