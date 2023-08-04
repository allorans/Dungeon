using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors.MeleeCreature.States{

public class Follow : State
{
    private MeleeCreatureController controller;
    private MeleeCreatureHelper helper;

    private readonly float updateInterval=1;
    private float ceaseFollowCooldown=0;

    private float updateCooldown=0;

    public Follow(MeleeCreatureController controller):base("Follow"){
    this.controller=controller;
    this.helper= controller.helper;
    }
    
    
public override void Enter()
    {
        base.Enter();
        
        updateCooldown=0;
    }

    public override void Exit()
        {
            base.Exit();
            
            controller.thisAgent.ResetPath();
        }

    public override void Update()
        {
            base.Update();

            if((updateCooldown-=Time.deltaTime)<0){
            
                updateCooldown=updateInterval;
                var player = GameManager.Instance.player;
                var playerPosition = player.transform.position;
                controller.thisAgent.SetDestination(playerPosition);

            }

            if((ceaseFollowCooldown-=Time.deltaTime)<0f){
                if(!helper.IsPlayerOnSight()){
                    controller.stateMachine.ChangeState(controller.idleState);
                    return;
                }
            }

            if(helper.GetDistanceToPlayer()<= controller.distanceToAttack){
                controller.stateMachine.ChangeState(controller.attackState);
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
