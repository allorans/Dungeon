using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs;

namespace Behaviors.LichBoss.States{

public class Dead : State
{
    private LichBossController controller;
    private LichBossHelper helper;



    public Dead(LichBossController controller):base("Dead"){
    this.controller=controller;
    this.helper= controller.helper;
    }
    
    
public override void Enter()
    {
        base.Enter();
        controller.thislife.isVulnerable=false;
        controller.thisAnimator.SetTrigger("tDead");
        Debug.Log("Dead!");


         GlobalEvents.Instance.InvokeOnGameWon(this,new GameWonArgs());
   
        
          
      
    }

    public override void Exit()
        {
            base.Exit();
            
            
        }

    public override void Update()
        {
            base.Update();

              


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
