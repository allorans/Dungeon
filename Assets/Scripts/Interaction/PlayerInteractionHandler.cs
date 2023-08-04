using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour
{
    private readonly float scaninterval=0.5f;
    private float scanCooldown=0f;

    private Interaction currentInteraction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((scanCooldown -= Time.deltaTime)<=0f){
            scanCooldown=scaninterval;
            ScanObject();

        }

        if(Input.GetKeyDown(KeyCode.E)){
            if(currentInteraction!=null){
               currentInteraction.Interact();
               ScanObject();
            }
        }


    }

    public Interaction GetNearestInteraction(Vector3 position){
    float closestDistance=-1;
    Interaction closestInteraction=null;

    var interactionList=GameManager.Instance.interactionList;
    foreach(Interaction interaction in interactionList){
        var distance=(interaction.transform.position - position).magnitude;
        var isAvaiable=interaction.IsAvaiable();
        var isCloseEnough=distance<=interaction.radius;
        var isCacheInvalid=closestDistance<0;
        if(isCloseEnough&&isAvaiable){
        if(isCacheInvalid||distance<closestDistance){
            closestDistance=distance;
            closestInteraction=interaction;

        }}
    }
    return closestInteraction;

   }



   public void ScanObject(){

    Interaction nearestInteraction=GetNearestInteraction(transform.position);
    if(nearestInteraction!=currentInteraction){
        currentInteraction?.SetActive(false);
        nearestInteraction?.SetActive(true);
        currentInteraction=nearestInteraction;

    }

   }


}
