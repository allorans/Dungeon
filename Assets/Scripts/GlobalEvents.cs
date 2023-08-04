using System;
using EventArgs;
using UnityEngine;

public class GlobalEvents: MonoBehaviour
{

    public static GlobalEvents Instance{get;private set;}
    // Start is called before the first frame update

    public event EventHandler<BossRoomOpenArgs>OnBossRoomOpen;
    public event EventHandler<BossRoomEnterArgs>OnBossRoomEnter;
    public event EventHandler<GameWonArgs>OnGameWon;
    public event EventHandler<GameOverArgs>OnGameOver;
    void Start()
    {
        
    }

    private void Awake(){

        if(Instance != null && Instance != this){
            Destroy(this);
        }else{
            Instance=this;
        }    
    }


    public void InvokeOnBossRoomOpen(object sender,BossRoomOpenArgs args){OnBossRoomOpen?.Invoke(sender,args);}
    public void InvokeOnBossRoomEnter(object sender,BossRoomEnterArgs args){OnBossRoomEnter?.Invoke(sender,args);}
    public void InvokeOnGameWon(object sender,GameWonArgs args){OnGameWon?.Invoke(sender,args);}
    public void InvokeOnGameOver(object sender,GameOverArgs args){OnGameOver?.Invoke(sender,args);}

    // Update is called once per frame
    void Update()
    {
        
    }
}
