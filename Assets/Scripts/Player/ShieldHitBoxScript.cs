using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHitBoxScript : MonoBehaviour
{
    public PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        playerController.OnShieldCollisionEnter(other);
    }

    // private void OnCollisionEnter(Collision collision) {
    //         playerController.OnShieldCollisionEnter(collision);
    //     }

}
    