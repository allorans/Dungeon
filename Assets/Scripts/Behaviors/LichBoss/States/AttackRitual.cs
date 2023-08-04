using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors.LichBoss.States{

public class AttackRitual : State
{
    private LichBossController controller;
    private LichBossHelper helper;

    private float endAttackCooldown;

    private IEnumerator attackCoroutine;

    public AttackRitual(LichBossController controller):base("AttackRitual"){
    this.controller=controller;
    this.helper= controller.helper;
    }
    
    
public override void Enter()
    {
        base.Enter();
        endAttackCooldown=controller.attackRitualDuration;
        // Debug.Log("Atacou com ritual!");
        // controller.stateMachine.ChangeState(controller.idleState);
        controller.thisAnimator.SetTrigger("tAttackRitual");
         helper.StartStateCoroutine(
        ScheduleAttack(controller.attackRitualDelay));
        
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
        var gameObject=Object.Instantiate(controller.ritualPrefab,controller.staffBottom.position,controller.ritualPrefab.transform.rotation);

        Object.Destroy(gameObject,10);

        if(helper.GetDistanceToPlayer()<=controller.distanceToRitual){
            var playerLife=GameManager.Instance.player.GetComponent<LifeScript>();
            playerLife.InflictDamage(controller.gameObject,controller.attackDamage);
        }
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
}


}
