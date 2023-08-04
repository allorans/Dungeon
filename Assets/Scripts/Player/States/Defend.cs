using UnityEngine;
namespace Player.States{
public class Defend : State
{
    private PlayerController controller;
    public Defend(PlayerController controller) : base("Defend"){
     this.controller=controller;   
    }

    public override void Enter()
    {
        base.Enter();
        controller.shieldHitBox.SetActive(true);
        controller.thisAnimator.SetBool("bDefend",true);
    }

    public override void Exit()
        {
            base.Exit();
            controller.shieldHitBox.SetActive(false);
            controller.thisAnimator.SetBool("bDefend",false);
        }

    public override void Update()
        {
            base.Update();

            if(!controller.hasDefenseInput){
                controller.stateMachine.ChangeState(controller.idleState);
            }
              
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

        }
    
    public override void FixedUpdate()
        {
            base.FixedUpdate();
            controller.RotateBodyToFaceInput();

        }
}}