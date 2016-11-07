using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public TankMotor player, playerTwo;
    public GameObject playerPrefab;
    public List<GameObject> enemies;
    public int remainingEnemies;

    public GameObject RemainingEnemiesGUI, RemainingEnemiesGUITwo;
    public GameObject ScoreGUI, ScoreGUITwo;
    public GameObject HealthGUI, HealthGUITwo;
	public GameObject LivesGUI, LivesGUITwo;
    public GameObject StatusGUI, StatusGUITwo;

	public Canvas canvOne, canvTwo;

    public GameObject[] EnemyTankTypes;

    private MapGenerator map;
    public Room[,] grid;

    void Start() {
        map = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
		if(PlayerPrefs.GetInt("TwoPlayers") == 1)SpawnTwoPlayer();
		else SpawnPlayer();
        for (int i = 0; i < EnemyTankTypes.Length; i++) {
            int x = Random.Range(0, map.rows);
            int z = Random.Range(0, map.cols);
            GameObject tempEnemy = Instantiate(EnemyTankTypes[i], grid[x,z].gameObject.transform.FindChild("EnemyTankSpawner").position, Quaternion.identity) as GameObject;
            int index = 0;//bad implementation with an array, change to a list eventually, can only use temporarily since I know there is exactly 4 waypoints
            foreach(Transform child in grid[x,z].transform) {
                if (child.CompareTag("Waypoint") && index < 4) {
                    tempEnemy.GetComponent<AIController>().waypoints[index] = child;
                    index++;
                }
            }
            enemies.Add(tempEnemy);
        }
        remainingEnemies = enemies.Count;
    }
    void Awake() {
        if (instance == null) instance = this;
        else {
            Debug.LogError("ERROR: There can only be one GameManager.");
            Destroy(gameObject);
        }
    }
    void Update() {
		
    }

    void OnGUI() {
	    ScoreGUI.GetComponent<Text>().text = "Score: " + player.data.score;
	   	RemainingEnemiesGUI.GetComponent<Text>().text = "Remaining Enemies: " + remainingEnemies;
	    HealthGUI.GetComponent<Text>().text = "Health: " + player.data.health;
		LivesGUI.GetComponent<Text>().text = "Lives: " + player.data.lives;

		if(PlayerPrefs.GetInt("TwoPlayers") == 1){
			ScoreGUITwo.GetComponent<Text>().text = "Score: " + playerTwo.data.score;
			RemainingEnemiesGUITwo.GetComponent<Text>().text = "Remaining Enemies: " + remainingEnemies;
			HealthGUITwo.GetComponent<Text>().text = "Health: " + playerTwo.data.health;
			LivesGUITwo.GetComponent<Text>().text = "Lives: " + playerTwo.data.lives;
		}
	}

    public void GameOver() {
		if(PlayerPrefs.GetInt("TwoPlayers") == 1){
			if(player.data.lives<=0&&playerTwo.data.lives<=0){
				LevelManagerScript.instance.LoadLevel("03 Lose Screen");
			}else if(playerTwo.data.lives<=0){
				StatusGUITwo.GetComponent<Text>().text = "GAME OVER";
				StatusGUITwo.SetActive(true);
			}else if(player.data.lives<=0){
				StatusGUI.GetComponent<Text>().text = "GAME OVER";
				StatusGUI.SetActive(true);
			}
		}
    }
    public void Victory() {
		if(player.data.score>=int.Parse(PlayerPrefs.GetString("HighScore")))PlayerPrefs.SetString("HighScore",player.data.score.ToString());
		if(playerTwo.data.score>=int.Parse(PlayerPrefs.GetString("HighScore")))PlayerPrefs.SetString("HighScore",playerTwo.data.score.ToString());
		PlayerPrefs.Save();
		LevelManagerScript.instance.LoadLevel("03 Win Screen");
    }
    public void SpawnPlayer() {
        int x = Random.Range(0, map.rows);
        int z = Random.Range(0, map.cols);
        GameObject playerObj = Instantiate(playerPrefab, grid[x, z].gameObject.transform.FindChild("PlayerSpawn").position, Quaternion.identity) as GameObject;
        player = playerObj.GetComponent<TankMotor>();

		Camera camOne = playerObj.GetComponentInChildren<Camera>();

		canvOne.GetComponent<Canvas>().worldCamera = camOne;
    }
	public void SpawnTwoPlayer(){
		int x = Random.Range(0,map.rows); 
		int xx = Random.Range(0,map.rows);
		int z = Random.Range(0,map.cols); 
		int zz = Random.Range(0,map.cols);
		GameObject playerObjOne = Instantiate(playerPrefab, grid[x,z].gameObject.transform.FindChild("PlayerSpawn").position,Quaternion.identity) as GameObject;
		GameObject playerObjTwo = Instantiate(playerPrefab,grid[xx,zz].gameObject.transform.FindChild("PlayerSpawn").position,Quaternion.identity)as GameObject;
		player = playerObjOne.GetComponent<TankMotor>();
		playerTwo = playerObjTwo.GetComponent<TankMotor>();

		Camera camOne = playerObjOne.GetComponentInChildren<Camera>();
		Camera camTwo = playerObjTwo.GetComponentInChildren<Camera>();

		camOne.rect = new Rect(0.0f,0.5f,1.0f,0.5f);
		camTwo.rect = new Rect(0.0f,0.0f,1.0f,0.5f);

		playerObjTwo.GetComponent<InputController>().scheme = InputController.InputScheme.ArrowKeys;

		canvTwo.gameObject.SetActive(true);

		canvOne.GetComponent<Canvas>().worldCamera=camOne;
		canvTwo.GetComponent<Canvas>().worldCamera=camTwo;
	}
	public void RespawnPlayer(){
		if(PlayerPrefs.GetInt("TwoPlayers") == 1){
			if(player.data.lives == player.data.prevLives-1){
				Debug.Log("spawning player 1");
				int x = Random.Range(0,map.rows);
				int z = Random.Range(0,map.cols);
				GameObject playerObjOne = Instantiate(playerPrefab, grid[x,z].gameObject.transform.FindChild("PlayerSpawn").position,Quaternion.identity) as GameObject;
				playerObjOne.GetComponent<TankData>().lives=player.data.lives;
				player = playerObjOne.GetComponent<TankMotor>();
				Camera camOne = playerObjOne.GetComponentInChildren<Camera>();
				camOne.rect = new Rect(0.0f,0.5f,1.0f,0.5f);
				canvOne.GetComponent<Canvas>().worldCamera=camOne;
			}else if(playerTwo.data.lives == playerTwo.data.prevLives-1){
				Debug.Log("spawning player 2");
				int x = Random.Range(0,map.rows);
				int z = Random.Range(0,map.cols);
				GameObject playerObjTwo = Instantiate(playerPrefab,grid[x,z].gameObject.transform.FindChild("PlayerSpawn").position,Quaternion.identity)as GameObject;
				playerObjTwo.GetComponent<TankData>().lives=playerTwo.data.lives;
				playerTwo = playerObjTwo.GetComponent<TankMotor>();
				Camera camTwo = playerObjTwo.GetComponentInChildren<Camera>();
				camTwo.rect = new Rect(0.0f,0.0f,1.0f,0.5f);
				playerObjTwo.GetComponent<InputController>().scheme = InputController.InputScheme.ArrowKeys;
				canvTwo.GetComponent<Canvas>().worldCamera=camTwo;
			}
		}else{
			SpawnPlayer();
		}
	}
}