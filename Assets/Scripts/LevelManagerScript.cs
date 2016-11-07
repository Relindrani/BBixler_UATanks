using UnityEngine;
using System.Collections;

public class LevelManagerScript : MonoBehaviour {
	public static LevelManagerScript instance;

	public int previousLevel;

	AudioSource music;

	void Start(){
		if(!instance)instance=this;
		else Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
		music = GameObject.Find("MusicManager").GetComponent<AudioSource>();
		Debug.Log(music.clip.name);
		music.volume=float.Parse(PlayerPrefs.GetString("MusicVolume"));
		music.ignoreListenerVolume = true;
		music.Play();
	}
	public void LoadLevel(string name){
		Application.LoadLevel(name);
		previousLevel=Application.loadedLevel;
	}
	public void QuitRequest(){
		Debug.Log ("Quit Game");
		Application.Quit();
	}
	public void LoadNextLevel(){
		Application.LoadLevel (Application.loadedLevel+1);
		music = GameObject.Find("MusicManager").GetComponent<AudioSource>();
		music.volume=float.Parse(PlayerPrefs.GetString("MusicVolume"));
		music.ignoreListenerVolume = true;
	}
	public void LoadPreviousLevel(){
		Application.LoadLevel(instance.previousLevel);
		music = GameObject.Find("MusicManager").GetComponent<AudioSource>();
		music.volume=float.Parse(PlayerPrefs.GetString("MusicVolume"));
		music.ignoreListenerVolume = true;
	}
}