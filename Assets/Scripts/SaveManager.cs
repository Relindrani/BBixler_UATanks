using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour {

    public Text music, sfx, highScore;
	public Toggle mapOfDay, twoPlayer;
	public int highScoreInt;

    void Awake() {
        Load();
		if(PlayerPrefs.HasKey("HighScore"))highScoreInt=int.Parse(PlayerPrefs.GetString("HighScore"));
		else highScoreInt = 0;
    }

    public void Save() {
        PlayerPrefs.SetString("MusicVolume", music.text);
        PlayerPrefs.SetString("SFXVolume", sfx.text);
        PlayerPrefs.SetInt("MapOfDay", mapOfDay.isOn ? 1 : 0);
        PlayerPrefs.SetInt("TwoPlayers", twoPlayer.isOn ? 1 : 0);
		highScore.text = highScoreInt.ToString();
		PlayerPrefs.SetString("HighScore", highScore.text);
        PlayerPrefs.Save();
    }
    public void Load() {
        music.text = PlayerPrefs.GetString("MusicVolume");
        sfx.text = PlayerPrefs.GetString("SFXVolume");
		AudioListener.volume = float.Parse(sfx.text)/100;
        mapOfDay.isOn = PlayerPrefs.GetInt("MapOfDay") == 1 ? true : false;
        twoPlayer.isOn = PlayerPrefs.GetInt("TwoPlayers") == 1 ? true : false;
		highScore.text = PlayerPrefs.GetString("HighScore");
    }
}
