using Behaviors;
using UnityEngine;
using System;
namespace BossBattle{
public class Intro: State
{
    private readonly float duration=3f;
    private float timeElapsed=0;
    public Intro():base("Intro"){}
        // Start is called before the first frame update
        public override void Enter()
        {
            base.Enter();
            timeElapsed=0;

            var gameManager=GameManager.Instance;

            gameManager.bossBattleParts.SetActive(true);


            var gameplayMusic=gameManager.gameplayMusic;
            gameManager.StartCoroutine(FadeAudioSource.StartFade(gameplayMusic,0,2f));
            

            var bossMusic=gameManager.bossMusic;
            var bossMusicVolume=bossMusic.volume;
            bossMusic.volume=0;
            gameManager.StartCoroutine(FadeAudioSource.StartFade(bossMusic,bossMusicVolume,0.5f));
            bossMusic.Play();

        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            timeElapsed+=Time.deltaTime;
            if(timeElapsed>=duration){
                var bossBattleHandler=GameManager.Instance.bossBattleHandler;
            bossBattleHandler.stateMachine.ChangeState(bossBattleHandler.stateBattle); 
            }
        }

    
    
}

}