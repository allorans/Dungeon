using UnityEngine;
using EventArgs;
using Item;
using System;
using UnityEngine.Events;

namespace Chest{
public class ChestScript : MonoBehaviour
{
    public Interaction interaction;

    public Item.Item item;

    private Animator thisAnimator;

    public GameObject itemHolder;

    public ChestOpenEvent onOpen=new();


    private void Awake() {
        thisAnimator=GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        interaction.OnInteraction += OnInteraction;
        interaction.SetActionText("Abra o baú!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnInteraction(object sender, InteractionEventArgs args){
            Debug.Log("Jogador acabou de interagir com o baú! Contendo item "+ item.displayName+ "!");
            interaction.SetAvaiable(false);
            thisAnimator.SetTrigger("tOpen");
            var itemObjectPrefab=item.objectPrefab;
            var position=itemHolder.transform.position;
            var rotation=itemHolder.transform.rotation;
            var itemObject=Instantiate(itemObjectPrefab,position,rotation);
            itemObject.transform.localScale=new Vector3(2,2,2);
            itemObject.transform.SetParent(itemHolder.transform);


            var itemType=item.itemType;
            if(itemType==ItemType.Key){
                GameManager.Instance.keys++;
            }else if(itemType==ItemType.BossKey){
                GameManager.Instance.hasBossKey=true;
            } else if(itemType==ItemType.Potion){
                var player=GameManager.Instance.player;
                var playerLife=player.GetComponent<LifeScript>();
                playerLife=player.GetComponent<LifeScript>();
                playerLife.Heal();
            }
            onOpen?.Invoke(gameObject);

            var gameplayUI=GameManager.Instance.gameplayUI;
            gameplayUI.AddObject(itemType);
        }
}


[Serializable] public class ChestOpenEvent:UnityEvent<GameObject>{}

}
