using UnityEngine;
namespace Player.States{
public class Walking:State{

    private PlayerController controller;
    private float footstepCooldown;

    public Walking(PlayerController controller) : base("Walking")
    {
       this.controller=controller; 
    }


    public override void Enter()
    {
        base.Enter();
        footstepCooldown = controller.footstepInterval;
        // Debug.Log("Entrou no Walking");
    }

    public override void Exit()
        {
            base.Exit();
            // Debug.Log("Saiu do Walking");
        }

    public override void Update()
        {
            base.Update();

            if(controller.hasJumpInput){

            controller.stateMachine.ChangeState(controller.jumpState);
            return;
            }

            if(controller.movementVector.IsZero()){

            controller.stateMachine.ChangeState(controller.idleState);
            return;
            }

            if(controller.AttemptToAttack()){
                return;
            }

            if(controller.hasDefenseInput){
                controller.stateMachine.ChangeState(controller.defendState);
            }

            float velocity = controller.thisRigidBody.velocity.magnitude;
            float velocityRate = velocity / controller.maxSpeed;
            footstepCooldown -= Time.deltaTime * velocityRate;
            if (footstepCooldown <= 0f) {
                footstepCooldown = controller.footstepInterval;
                var audioClip = controller.footstepSounds[Random.Range(0, controller.footstepSounds.Count - 1)];
                var volumeScale = Random.Range(0.8f, 1f);
                controller.footstepAudioSource.PlayOneShot(audioClip, volumeScale);
            }

        }

        public override void LateUpdate()
        {
            base.LateUpdate();

        }
    
    public override void FixedUpdate()
        {
            base.FixedUpdate();
            Vector3 walkVector= new Vector3(controller.movementVector.x,0,controller.movementVector.y);
            Camera camera=Camera.main;
            walkVector = controller.GetForward()*walkVector;
            walkVector = Vector3.ProjectOnPlane(walkVector,controller.slopeNormal);
            walkVector*=controller.movementSpeed;

            // controller.transform.Translate(walkVector);
            controller.thisRigidBody.AddForce(walkVector, ForceMode.Force);
            controller.RotateBodyToFaceInput();


}
}}