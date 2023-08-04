using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BossBattle;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance{get; private set;}

    [HideInInspector] public bool isGameOver;
    public GameObject player;

  
     public List<Interaction> interactionList;

    [Header("Rendering\n")]
    public Camera worldUiCamera;

    [Header("Physics\n")]

    [SerializeField]public LayerMask groundLayer;
    

    [Header("Inventory\n")]
    public int keys;
    public bool hasBossKey;


    [Header("Boss\n")]
    public GameObject boss;
    public BossBattleHandler bossBattleHandler;
    public GameObject bossBattleParts;

    public GameObject bossDeathSequence;


    [Header("Music\n")]
    public AudioSource gameplayMusic;
    public AudioSource bossMusic;
    public AudioSource ambienceMusic;

    [Header("UI\n")]
    public GameplayUI gameplayUI;
    
    

    void Awake() {

        if(Instance != null && Instance != this){
            Destroy(this);
        }else{
            Instance=this;
        }     

        
    }

    private void Start() {
    bossBattleHandler= new BossBattleHandler();   

    var musicTargetVolume= gameplayMusic.volume;
    gameplayMusic.volume=0;
    gameplayMusic.Play();
    StartCoroutine(FadeAudioSource.StartFade(gameplayMusic,musicTargetVolume,1f));

    var ambienceTargetVolume= ambienceMusic.volume;
    ambienceMusic.volume=0;
    ambienceMusic.Play();
    StartCoroutine(FadeAudioSource.StartFade(ambienceMusic,ambienceTargetVolume,1f));


    GlobalEvents.Instance.OnGameOver += (sender,args)=>isGameOver=true;

    }
    // Start is called before the first frame update
   void Update(){
    bossBattleHandler.Update();
   
   }

   

   
    
}
