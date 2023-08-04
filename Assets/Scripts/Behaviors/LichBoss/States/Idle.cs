using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Behaviors.LichBoss.States{

public class Idle : State
{
    private LichBossController controller;
    private LichBossHelper helper;

    private float stateTime=0;


    public Idle(LichBossController controller):base("Idle"){
    this.controller=controller;
    this.helper= controller.helper;
    }
    
    
public override void Enter()
    {
        base.Enter();
        stateTime=0;
        Debug.Log("Entrou no state "+this.name);
    }

    public override void Exit()
        {
            base.Exit();
            
        }

    public override void Update()
        {
            base.Update();
            stateTime+=Time.deltaTime;

            if(GameManager.Instance.isGameOver) return;

            if(stateTime>=controller.idleDuration){
            controller.stateMachine.ChangeState(controller.followState);
            return;}


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
