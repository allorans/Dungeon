using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDeathSequence : MonoBehaviour
{
    public void SwitchToCreditsScreen(){
        SceneManager.LoadScene("Scenes/Credits");
    }

    
}
