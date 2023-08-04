using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventArgs;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    public GameObject objectToEnable;
    public float gameOverDuration=6f;
    private bool isTriggered;
    // Start is called before the first frame update
    void Start()
    {
        GlobalEvents.Instance.OnGameOver+=OnGameOver;
    }

    private void OnGameOver(object sender,GameOverArgs args){
        if(isTriggered) return;
        isTriggered=true;

        objectToEnable.SetActive(true);

        StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene(){
        yield return new WaitForSeconds(gameOverDuration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
