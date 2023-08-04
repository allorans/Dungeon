using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EventArgs;

public class Interaction : MonoBehaviour
{

    public GameObject widgetPrefab;
    [SerializeField] private Vector3 widgetOffset;
    public float radius=10f;
    private GameObject widget;

    private bool isAvaiable=true;
    private bool isActive;


    public event EventHandler<InteractionEventArgs> OnInteraction;

    private void OnEnable() {
        GameManager.Instance.interactionList.Add(this);
    }

    private void OnDisable() {
        GameManager.Instance.interactionList.Remove(this);
    }

    void Awake() {
       widget=Instantiate(widgetPrefab,transform.position+widgetOffset,widgetPrefab.transform.rotation);
        widget.transform.SetParent(gameObject.transform,true);
         
    }

    private void Start() {

        // widget=Instantiate(widgetPrefab,transform.position+widgetOffset,widgetPrefab.transform.rotation); 
        // widget.transform.SetParent(gameObject.transform,true);

        var worldUiCamera = GameManager.Instance.worldUiCamera;      
        var canvas=widget.GetComponent<Canvas>();
        if(canvas!=null){
            canvas.worldCamera=worldUiCamera;
        }

         var interactionWidgetComponent=widget.GetComponent<InteractionWidget>();
        if(interactionWidgetComponent!=null){
            interactionWidgetComponent.worldUiCamera=worldUiCamera;
        }

        
        
    }
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
    
    }

    public bool IsActive(){
        return isActive;
    }

    public void SetActive(bool isActive){
        var wasActive=this.isActive;
            this.isActive=isActive;

            var interactionWidget=widget.GetComponent<InteractionWidget>();
            if(isActive){
                interactionWidget.Show();
            }else{
                interactionWidget.Hide();
            }

        
    }

    public bool IsAvaiable(){
        return isAvaiable;
    }

    public void SetAvaiable(bool isAvaiable){
        this.isAvaiable=isAvaiable;
    }

    public void Interact(){
        OnInteraction?.Invoke(this,new InteractionEventArgs());
        
    }

   public void SetActionText(string text){
    var interactionWidget=widget.GetComponent<InteractionWidget>();
        interactionWidget.SetActionText(text);
   }

}
