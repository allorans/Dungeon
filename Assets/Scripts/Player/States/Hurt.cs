using UnityEngine;
namespace Player.States{
public class Hurt : State
{
    private PlayerController controller;
    private float timePassed;
    public Hurt(PlayerController controller) : base("Hurt"){
     this.controller=controller;   
    }

    public override void Enter()
    {
        base.Enter();

        timePassed=0;

        controller.thisLife.isVulnerable=false;

        controller.thisAnimator.SetTrigger("tHurt");

        var gameplayUI=GameManager.Instance.gameplayUI;
        gameplayUI.playerHealthBar.SetHealth(controller.thisLife.health);
    }

    public override void Exit()
        {
            base.Exit();
            controller.thisLife.isVulnerable=true;
        }

    public override void Update()
        {
            base.Update();

            if(controller.thisLife.IsDead()){
                    controller.stateMachine.ChangeState(controller.deadState);
                    return;
                }

            timePassed+=Time.deltaTime;

            if(timePassed>=controller.hurtDuration){
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
}}