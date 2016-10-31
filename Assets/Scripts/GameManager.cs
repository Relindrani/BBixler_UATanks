using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public TankMotor player;
    public GameObject playerPrefab;
    public List<GameObject> enemies;
    public int remainingEnemies;

    public GameObject RemainingEnemiesGUI;
    public GameObject ScoreGUI;
    public GameObject HealthGUI;
    public GameObject StatusGUI;
    public GameObject ContinueGUI;

    private bool done = false;

    public GameObject[] EnemyTankTypes;

    private MapGenerator map;
    public Room[,] grid;

    void Start() {
        map = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
        SpawnPlayer();
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
        if (done) if (Input.GetKey(KeyCode.Space)) Application.Quit();
    }

    void OnGUI() {
        ScoreGUI.GetComponent<Text>().text = "Score: " + player.data.score;
        RemainingEnemiesGUI.GetComponent<Text>().text = "Remaining Enemies: " + remainingEnemies;
        HealthGUI.GetComponent<Text>().text = "Health: " + player.data.health;
    }

    public void GameOver() {
        StatusGUI.GetComponent<Text>().text = "GAME OVER";
        StatusGUI.SetActive(true);
        ContinueGUI.SetActive(true);
        done = true;
    }
    public void Victory() {
        StatusGUI.GetComponent<Text>().text = "VICTORY";
        StatusGUI.SetActive(true);
        ContinueGUI.SetActive(true);
        done = true;
    }
    public void SpawnPlayer() {
        int x = Random.Range(0, map.rows);
        int z = Random.Range(0, map.cols);
        GameObject playerObj = Instantiate(playerPrefab, grid[x, z].gameObject.transform.FindChild("PlayerSpawn").position, Quaternion.identity) as GameObject;
        player = playerObj.GetComponent<TankMotor>();
    }
}