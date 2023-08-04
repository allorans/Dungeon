using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigationexample : MonoBehaviour
{
    // Start is called before the first frame update
   public GameObject target;
   private NavMeshAgent thisAgent;
   
    void Awake() {
    thisAgent=GetComponent<NavMeshAgent>();
   }
    // Update is called once per frame
    void Update()
    {
   thisAgent.SetDestination(target.transform.position);     
    }
}
