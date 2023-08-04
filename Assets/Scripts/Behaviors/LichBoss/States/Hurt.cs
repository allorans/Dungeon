using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors.LichBoss.States{

public class Hurt : State
{
    private LichBossController controller;
    private LichBossHelper helper;

    private float timePassed;

    public Hurt(LichBossController controller):base("Hurt"){
    this.controller=controller;
    this.helper= controller.helper;
    }
    
    
public override void Enter()
    {
        base.Enter();
        timePassed=0;
        controller.thislife.isVulnerable=false;
        controller.thisAnimator.SetTrigger("tHurt");
        Debug.Log("Hurt!");
        
    }

    public override void Exit()
        {
            base.Exit();
            controller.thislife.isVulnerable=true;
        }

    public override void Update()
        {
            base.Update();

            timePassed+=Time.deltaTime;
            
            if(timePassed>=controller.hurtDuration){
                if(controller.thislife.IsDead()){
                    controller.stateMachine.ChangeState(controller.deadState);
                }else{
                controller.stateMachine.ChangeState(controller.idleState);}
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


}


}
