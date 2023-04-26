using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Scavenger v2
[DefaultExecutionOrder(1100)]
public class MusicManager : MonoBehaviour
{
    private float currentVolume;
    private float desiredVolume;
    private float volumeMax;
    private float volumeMin = 0f;
    private float fadeTime = 5f;
    
    public static MusicManager Instance;

    [SerializeField]
        public AudioSource gameMusic;
        
    public AudioMixer mixer;

    [SerializeField]
        private string volumeName;

    
    [SerializeField] VolumeSliders volumeSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            gameMusic = GetComponent<AudioSource>();

            if(MainManager.Instance.isSavedData)
                gameMusic.volume = MainManager.Instance.savedVolume;
            //Debug.Log("Music Manager is Awake!  Volume: " + gameMusic.volume);
            else
                gameMusic.volume = MainManager.Instance.defaultVolume;

            volumeMax = gameMusic.volume;
            if(string.IsNullOrEmpty(MainManager.Instance.pilotName))
                MainManager.Instance.savedVolume = volumeMax;

            //PlayGameMusic();
            
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Called from Wave Manager when the wave ends, and Game Manager when the game ends
    public void StartFadeOut()
    {
        volumeMax = gameMusic.volume; //Save the game volume before fading out
        currentVolume = gameMusic.volume;         
        desiredVolume = volumeMin;
        StartCoroutine(FadeOut()); 
    }
 
    IEnumerator FadeOut() 
    {        
        while (currentVolume > desiredVolume) 
        {
            Time.timeScale = 1;
            //Debug.Log ("FadeOut currentVolume:  " + currentVolume);
            currentVolume = Mathf.MoveTowards(currentVolume, desiredVolume, fadeTime * Time.deltaTime);
            gameMusic.volume = currentVolume;

            yield return null;
        }
        gameMusic.Pause();
        Time.timeScale = 0;
        yield break;
    }
    
    // Called from Wave Manager when the new wave starts
    public void StartFadeIn()
    {
        currentVolume = volumeMin;         
        desiredVolume = volumeMax; // The game volume was saved before fading out
        gameMusic.volume = volumeMin;
        gameMusic.Play();
        StartCoroutine(FadeIn()); 
    }
    IEnumerator FadeIn() 
    {

        //yield return new WaitForSeconds(fadeTime); 
        while (currentVolume < desiredVolume) 
        {
            //Debug.Log ("FadeIn currentVolume:  " + currentVolume);
            currentVolume = Mathf.MoveTowards(currentVolume, desiredVolume, fadeTime * Time.deltaTime);
            gameMusic.volume = currentVolume;

            yield return null;
        }
        yield break;
    }
    
    //Function Call :
    //MusicManager.Instance.PlayGameMusic(); 
    //MusicManager.Instance.PauseGameMusic(); 
    //MusicManager.Instance.StopGameMusic();

    public void PlayGameMusic()
    {
        gameMusic.volume = volumeMax;
        //Debug.Log("Play game music - Volume = " + gameMusic.volume);
        gameMusic.loop = true;
        gameMusic.Stop();
        gameMusic.Play();
    }
    
    public void PauseGameMusic()
    {
        gameMusic.Pause();
    }

    public void StopGameMusic()
    {
        gameMusic.Stop();
    }

    public void ResetVolumeSliders()
    {
    
        //GameObject VolumeSliders = gameObject.transform.GetChild(0).gameObject;
        volumeSlider = GameObject.Find("Volume Sliders").GetComponent<VolumeSliders>();

        Debug.Log("Reset Volume Sliders, volume: " + MusicManager.Instance.gameMusic.volume);

        volumeSlider.ResetSlider(MusicManager.Instance.gameMusic.volume);
    }
}
