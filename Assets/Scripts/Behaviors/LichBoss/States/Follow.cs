using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors.LichBoss.States{

public class Follow : State
{
    private LichBossController controller;
    private LichBossHelper helper;

    private readonly float attackAttemptInterval=0.5f;

    private readonly float targetUpdateInterval=0.5f;
    
    private  float targetUpdateCooldown=0f;
    private float attackAttemptCooldown=0f;
    private float ceaseFollowCooldown=0f;

    

    public Follow(LichBossController controller):base("Follow"){
    this.controller=controller;
    this.helper= controller.helper;
    }
    
    
public override void Enter()
    {
        base.Enter();
        
       attackAttemptCooldown=attackAttemptInterval;
       targetUpdateCooldown=0f;
       ceaseFollowCooldown=controller.ceaseFollowInterval;
       Debug.Log("Entrou no state "+this.name);

    }

    public override void Exit()
        {
            base.Exit();
          controller.thisAgent.ResetPath();
        }

    public override void Update()
        {
            base.Update();

            if((targetUpdateCooldown-=Time.deltaTime)<0){
            
                targetUpdateCooldown=targetUpdateInterval;
                var player = GameManager.Instance.player;
                var playerPosition = player.transform.position;
                controller.thisAgent.SetDestination(playerPosition);

            }

            if((attackAttemptCooldown-=Time.deltaTime)<=0){
                attackAttemptCooldown=attackAttemptInterval;

                var distanceToPlayer=helper.GetDistanceToPlayer();
                var isCloseEnoughToRitual= distanceToPlayer <= controller.distanceToRitual;
               
                    if(isCloseEnoughToRitual){
                        controller.stateMachine.ChangeState(controller.attackRitualState);
                        return;
                    }}


            if((ceaseFollowCooldown-=Time.deltaTime)<=0f){
                State newState=helper.HasLowHealth()?controller.attackSuperState : controller.attackNormalState;
                controller.stateMachine.ChangeState(newState);
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
