using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{
    public Animator thisAnimator;
    public float fadeOutDuration;


    private bool isFadingOut;
    public void FadeOut(){

        if(isFadingOut) return;
        isFadingOut=true;

        thisAnimator.enabled=true;

        StartCoroutine(TransitionToNextScene());

    }

    private IEnumerator TransitionToNextScene(){
        yield return new WaitForSeconds(fadeOutDuration);

        SceneManager.LoadScene("Scenes/SampleScene");
    }
}
