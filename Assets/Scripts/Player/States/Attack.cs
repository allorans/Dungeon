using UnityEngine;
namespace Player.States{
public class Attack : State
{
    private PlayerController controller;

    public int stage =1;

    private float stateTime;

    private bool firstFixedUpdate;

    public Attack(PlayerController controller) : base("Attack"){
     this.controller=controller;   
    }

    public override void Enter()
    {
        base.Enter();

        if(stage<=0||stage>controller.attackStages){
            controller.stateMachine.ChangeState(controller.idleState);
            return;
        }
        controller.thisAnimator.SetTrigger("tAttack"+stage);
        stateTime=0;
        firstFixedUpdate=true;

        controller.swordHitBox.SetActive(true);
    }

    public override void Exit()
        {
            base.Exit();
            controller.swordHitBox.SetActive(false);
            
        }

    public override void Update()
        {
            base.Update();
            stateTime += Time.deltaTime;

           if(controller.AttemptToAttack()){
                return;
            }

            if(IsStageExpired()){
            controller.stateMachine.ChangeState(controller.idleState);
            return;}
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

        }
    
    public override void FixedUpdate()
        {
            base.FixedUpdate();

            if(firstFixedUpdate){
                firstFixedUpdate=false;
                controller.RotateBodyToFaceInput(1);

                var impulseValue = controller.attackStageInpulses[stage -1];
                var impulseVector = controller.thisRigidBody.rotation*Vector3.forward;
                impulseVector*=impulseValue;
                controller.thisRigidBody.AddForce(impulseVector,ForceMode.Impulse);
            
            }
        }


    public bool CanSwitchStages(){
        var isLastState = stage == controller.attackStages;
        var stageDuration = controller.attackStageDuration[stage-1];
        var stageMaxInterval = isLastState ? 0 :controller.attackStageMaxIntervals[stage-1];
        var maxStageDuration = stageDuration+stageMaxInterval;

        return !isLastState && stateTime >= stageDuration && stateTime <= maxStageDuration;
    }


    public bool IsStageExpired(){
        var isLastState = stage == controller.attackStages;
        var stageDuration = controller.attackStageDuration[stage-1];
        var stageMaxInterval = isLastState ? 0 :controller.attackStageMaxIntervals[stage-1];
        var maxStageDuration = stageDuration+stageMaxInterval;

        return stateTime > maxStageDuration;
    }

}}