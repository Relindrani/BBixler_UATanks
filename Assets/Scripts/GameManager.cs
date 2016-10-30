using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public TankMotor player;
    public GameObject[] enemies;
    public int remainingEnemies;

    public GameObject RemainingEnemiesGUI;
    public GameObject ScoreGUI;
    public GameObject HealthGUI;
    public GameObject StatusGUI;
    public GameObject ContinueGUI;

    private bool done = false;

	void Start() {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        remainingEnemies = enemies.Length;
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
}
