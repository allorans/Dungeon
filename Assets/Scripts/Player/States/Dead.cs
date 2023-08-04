using UnityEngine;
using EventArgs;
namespace Player.States{
public class Dead : State
{
    private PlayerController controller;
    public Dead(PlayerController controller) : base("Dead"){
     this.controller=controller;   
    }

    public override void Enter()
    {
        base.Enter();
        controller.thisAnimator.SetTrigger("tGameOver");

        controller.thisLife.isVulnerable=false;

        GlobalEvents.Instance.InvokeOnGameOver(this,new GameOverArgs());
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
}}