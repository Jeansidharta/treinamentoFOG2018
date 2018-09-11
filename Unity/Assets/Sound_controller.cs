using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound_controller : MonoBehaviour {

    [SerializeField] Slider MasterVolume;
    [SerializeField] Slider MusicVolume;
    [SerializeField] Slider SoundFX;
    [SerializeField] AudioClip skeletondeath;
    [SerializeField] AudioClip humandeath;
    [SerializeField] AudioClip evadeclip;

    public int isMenu;
    public int isGame; 

    private GameObject mainCamera;


    public AudioSource menuMusic;
    static public float SoundFXVolume = 1;
    static public float musicVolume = 1;
    static public float soundCoef = 1;
    public float mastervol;
    public float musicvol;
    public float soundfxvol;

    // Use this for initialization
    void Start () {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if( isMenu == 1 ) menuMusic = mainCamera.GetComponent<AudioSource>();
        else if(isGame == 1)
        {
            musicvol = musicVolume;
            mastervol = soundCoef;
            soundfxvol = SoundFXVolume;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (isMenu == 1)
        {
            soundCoef = MasterVolume.value / 100f;
            menuMusic.volume = soundCoef * MusicVolume.value / 100f;
            SoundFXVolume = SoundFX.value / 100f;
            musicVolume = MusicVolume.value / 100f;
        }   
	}

    public void playDeath(int team)
    {
        mainCamera.GetComponent<AudioSource>().volume = mastervol * soundfxvol;
        if (team == 0)
        {
            mainCamera.GetComponent<AudioSource>().PlayOneShot(humandeath);
        }
        else if(team == 1)
        {
            mainCamera.GetComponent<AudioSource>().PlayOneShot(skeletondeath);
        }
    }

    public void playEvade()
    {
        mainCamera.GetComponent<AudioSource>().volume = mastervol * soundfxvol;
        mainCamera.GetComponent<AudioSource>().PlayOneShot(evadeclip);
    }
}
