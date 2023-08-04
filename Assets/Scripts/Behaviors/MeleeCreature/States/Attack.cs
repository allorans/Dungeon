using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors.MeleeCreature.States{

public class Attack : State
{
    private MeleeCreatureController controller;
    private MeleeCreatureHelper helper;

    private float endAttackCooldown;

    private IEnumerator attackCoroutine;

    public Attack(MeleeCreatureController controller):base("Attack"){
    this.controller=controller;
    this.helper= controller.helper;
    }
    
    
public override void Enter()
    {
        base.Enter();
        endAttackCooldown=controller.attackDuration;

        Debug.Log("Scheduling Attack!");
        attackCoroutine=ScheduleAttack();
        controller.StartCoroutine(attackCoroutine);
        controller.thisAnimator.SetTrigger("tAttack");

        controller.slimeAudioSource.SetActive(true);
        
    }

    public override void Exit()
        {
            base.Exit();

            Debug.Log("Cancelling Attack!");
            controller.slimeAudioSource.SetActive(false);
            if(attackCoroutine!=null){
                controller.StopCoroutine(attackCoroutine);
            }
        }

    public override void Update()
        {
            base.Update();
            helper.FacePlayer();
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

    private IEnumerator ScheduleAttack(){
        yield return new WaitForSeconds(controller.damageDelay);
        PerformAttack();
    }
    private void PerformAttack(){
        Debug.Log("Performing Attack!");

        var origin=controller.transform.position;
        var direction=controller.transform.rotation*Vector3.forward;
        var radius=controller.attackRadius;
        var maxDistance=controller.attackSphereRadius;


        var attackPosition=direction*radius+origin;
        var layerMask=LayerMask.GetMask("Player");
        Collider[] colliders=Physics.OverlapSphere(attackPosition,maxDistance,layerMask);

        foreach(var collider in colliders){


            var hitObject=collider.gameObject;
            Debug.Log("Hit something: " + hitObject.name);
            

                var hitLifeScript=hitObject.GetComponent<LifeScript>();
                if(hitLifeScript!=null){
                    var attacker=controller.gameObject;
                    var damage=controller.attackDamage;

                    hitLifeScript.InflictDamage(attacker,damage);
                }

            
        
        }
        
    }
}


}
