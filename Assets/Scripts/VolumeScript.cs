using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VolumeScript : MonoBehaviour {

	public Text musicVolume, sfxVolume;
	public float musicVolumeFloat = 1.0f;
	AudioSource music;//object playing music

	void Start(){
		music=GameObject.Find("MusicManager").GetComponent<AudioSource>();//find the music
		musicVolumeFloat=float.Parse(PlayerPrefs.GetString("MusicVolume"))/100;//assign the value from playerprefs
	}
	public void increaseMusicVolume(){
		musicVolumeFloat+=0.05f;
		if(musicVolumeFloat>=1.0f)musicVolumeFloat=1.0f;
		updateMusicVolume();
	}
	public void reduceMusicVolume(){
		musicVolumeFloat-=0.05f;
		if(musicVolumeFloat<=0.0f)musicVolumeFloat=1.0f;
		updateMusicVolume();
	}
	void updateMusicVolume(){
		musicVolume.text=(Mathf.Round (musicVolumeFloat*100)).ToString();
		music.volume=musicVolumeFloat;
	}
    public void increaseSFXVolume() {
        AudioListener.volume += .05f;
        if (AudioListener.volume >= 1.0f) AudioListener.volume = 1.0f;
        updateSFXVolume();
    }
    public void reduceSFXVolume() {
        AudioListener.volume -= .05f;
        if (AudioListener.volume <= 0.0f) AudioListener.volume = 0.0f;
        updateSFXVolume();
    }
    void updateSFXVolume() {
        sfxVolume.text = (Mathf.Round(AudioListener.volume * 100)).ToString();
    }
}