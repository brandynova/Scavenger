using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource gameMusic;

    private float currentVolume;
    private float desiredVolume;
    private float volumeMax;
    private float volumeMin = 0f;

    // Start is called before the first frame update
    void Start()
    {
        gameMusic = GetComponent<AudioSource>();
        volumeMax = gameMusic.volume;
    }

    // Called from Wave Manager when the wave ends, and Game Manager when the game ends
    public void StartFadeOut(float fadeTime)
    {
        currentVolume = volumeMax;         
        desiredVolume = volumeMin;
        StartCoroutine(FadeOut(fadeTime)); 
    }
 
    IEnumerator FadeOut(float fadeTime) 
    {
        //yield return new WaitForSeconds(fadeTime/2f); 
        
        while (currentVolume > desiredVolume) 
        {
            Time.timeScale = 1;
            //Debug.Log ("FadeOut currentVolume, fadeTime * Time.deltaTime:  " + currentVolume + ", " + fadeTime * Time.deltaTime);
            currentVolume = Mathf.MoveTowards(currentVolume, desiredVolume, fadeTime * Time.deltaTime);
            gameMusic.volume = currentVolume;

            yield return null;
        }
        gameMusic.Pause();
        Time.timeScale = 0;
        yield break;
    }
    
    // Called from Wave Manager when the new wave starts
    public void StartFadeIn(float fadeTime)
    {
        currentVolume = volumeMin;         
        desiredVolume = volumeMax;
        gameMusic.volume = volumeMin;
        gameMusic.Play();
        StartCoroutine(FadeIn(fadeTime)); 
    }
    IEnumerator FadeIn(float fadeTime) 
    {
        //yield return new WaitForSeconds(fadeTime); 
        while (currentVolume < desiredVolume) 
        {
            //Debug.Log ("FadeIn currentVolume, fadeTime * Time.deltaTime:  " + currentVolume + ", " + fadeTime * Time.deltaTime);
            currentVolume = Mathf.MoveTowards(currentVolume, desiredVolume, fadeTime * Time.deltaTime);
            gameMusic.volume = currentVolume;

            yield return null;
        }
        //Time.timeScale = 0;
        yield break;
    }
}
