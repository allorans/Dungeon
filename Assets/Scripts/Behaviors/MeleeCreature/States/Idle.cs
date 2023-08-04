using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors.MeleeCreature.States{

public class Idle : State
{
    private MeleeCreatureController controller;
    private MeleeCreatureHelper helper;

    private float searchcooldown;

    public Idle(MeleeCreatureController controller):base("Idle"){
    this.controller=controller;
    this.helper= controller.helper;
    }
    
    
public override void Enter()
    {
        base.Enter();
        searchcooldown=controller.targetSearchInterval;
    }

    public override void Exit()
        {
            base.Exit();
            
        }

    public override void Update()
        {
            base.Update();

            if(GameManager.Instance.isGameOver) return;

            searchcooldown-=Time.deltaTime;
            if(searchcooldown<0){
                searchcooldown=controller.targetSearchInterval;


                if(helper.IsPlayerOnSight()){
                    controller.stateMachine.ChangeState(controller.followState);
                    return;
                }
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
