using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyantScript : MonoBehaviour
{
    public float underwaterDrag=3f;

    public float underwaterAngularDrag=1f;

    public float airDrag=0f;

    public float airAngularDrag=0.05f;

    

    public float buoyancyForce=10f;
    private Rigidbody thisRigidbody;

    private bool hasTouchedWater;
    // Start is called before the first frame update
    void Awake() {
        thisRigidbody=GetComponent<Rigidbody>();
    }
    void FixedUpdate() {
        float diffY=transform.position.y;
        bool isUnderWater=diffY<0;

        if(isUnderWater){
            hasTouchedWater=true;
        }

        if(!isUnderWater){
            return;
        }


        if(isUnderWater){
        Vector3 vector= Vector3.up * buoyancyForce * -diffY;
        thisRigidbody.AddForce(vector, ForceMode.Acceleration);}

        thisRigidbody.drag=isUnderWater? underwaterDrag :airDrag;
        thisRigidbody.angularDrag=isUnderWater? underwaterAngularDrag :airAngularDrag;
    }
    


}
