using UnityEngine;
using System;
using Behaviors;
using EventArgs;
namespace BossBattle{

public class BossBattleHandler
{
    
public StateMachine stateMachine;
public Disabled stateDisable;
public Waiting stateWaiting;
public Intro stateIntro;
public Battle stateBattle;
public BossDefeated stateDefeated;
public BossVictorious stateVictorious;

public  BossBattleHandler(){

stateMachine=new StateMachine();
 stateDisable= new Disabled();
 stateWaiting= new Waiting();
 stateIntro = new Intro();
stateBattle=new Battle();
stateDefeated=new BossDefeated();
stateVictorious=new BossVictorious();
stateMachine.ChangeState(stateDisable);


GameManager.Instance.bossBattleParts.SetActive(false);

var globalEvents=GlobalEvents.Instance;

globalEvents.OnBossRoomOpen+=(sender,args)=> stateMachine.ChangeState(stateWaiting);
globalEvents.OnBossRoomEnter+=(sender,args)=> stateMachine.ChangeState(stateIntro);
globalEvents.OnGameOver+=(sender,args)=> stateMachine.ChangeState(stateVictorious);
globalEvents.OnGameWon+=(sender,args)=> stateMachine.ChangeState(stateDefeated);

}

public void Update() {
    stateMachine.Update();
}

public bool IsActive(){
    return stateMachine.currentStateName==stateBattle.name;
}


public bool IsInCutscene(){
    return stateMachine.currentStateName==stateIntro.name;
}

}
}