using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs;
using Item;

public class DoorScript : MonoBehaviour
{

public Interaction interaction;
public Item.Item requiredkey;
private Animator thisAnimator;
private bool isOpen;


private void Awake() {
        thisAnimator=GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
      interaction.OnInteraction += OnInteraction;
      interaction.SetActionText("Abra a porta!"); 
    }

    // Update is called once per frame
    void Update()
    {
    if(!isOpen){
     var hasKey=false;
     if(requiredkey==null){
        hasKey=true;
     }   else if(requiredkey.itemType==ItemType.Key){
        hasKey=GameManager.Instance.keys>0;
     }   else if(requiredkey.itemType==ItemType.BossKey){
        hasKey=GameManager.Instance.hasBossKey;
     }
     
     interaction.SetAvaiable(hasKey);

     }
    }

    private void OnInteraction(object sender, InteractionEventArgs args){
            Debug.Log("Jogador acabou de interagir com a porta!");
            if(!isOpen){
            OpenDoor();
            }else{
            CloseDoor();
            }
            
        }

    private void OpenDoor(){

        isOpen=true;

        if(requiredkey!=null){
     if(requiredkey.itemType==ItemType.Key){
        GameManager.Instance.keys--;
     }   else if(requiredkey.itemType==ItemType.BossKey){
        GameManager.Instance.hasBossKey=false;
     }
        }

   
      var gameplayUI=GameManager.Instance.gameplayUI;
      gameplayUI.RemoveObject(requiredkey.itemType);
       

      interaction.SetAvaiable(false);

      thisAnimator.SetTrigger("tOpen");
            
      var isBossDoor=requiredkey.itemType==ItemType.BossKey;
      if(isBossDoor){
         GlobalEvents.Instance.InvokeOnBossRoomOpen(this,new BossRoomOpenArgs());
      }
    }

    private void CloseDoor(){

        isOpen=false;
        // interaction.SetAvaiable(false);
            thisAnimator.SetTrigger("tClose");
            

    }

}
