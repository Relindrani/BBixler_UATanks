using UnityEngine;
using System.Collections;
using System;

public class MapGenerator : MonoBehaviour {

    public int mapSeed;

    public int rows;
    public int cols;

    private float roomWidth = 50.0f;
    private float roomHeight = 50.0f;

    public GameObject[] gridPrefabs;

    public bool isMapOfTheDay;

    void Start() {
		isMapOfTheDay = PlayerPrefs.GetInt("MapOfDay") == 1 ? true : false;
        if (isMapOfTheDay) mapSeed = DateToInt(DateTime.Now.Date);//if map of the day is true, it takes priority even if isRandom is true
        else mapSeed = DateToInt(DateTime.Now);//if mapoftheday is false, set random seed
        GenerateGrid();
    }
	void Update () {
	
	}
    public void GenerateGrid() {
        UnityEngine.Random.seed = mapSeed;//set seed

        GameManager.instance.grid = new Room[cols, rows];//empty grid

        for(int i = 0; i < rows; i++) {//rows
            for(int j = 0; j < cols; j++) {//columns
                float xPos = roomWidth * j;//x position of room tile
                float zPos = roomHeight * i;//z position of room tile
                Vector3 newPos = new Vector3(xPos, 0.0f, zPos);//position of room tile

                GameObject tempObj = Instantiate(RandomRoomPrefab(), newPos, Quaternion.identity) as GameObject;//temporary gameobject of the room

                tempObj.transform.parent = this.transform;//set parent
                tempObj.name = "Room_" + j + "," + i;//name room

                Room tempRoom = tempObj.GetComponent<Room>();//get room component
                if (i == 0) tempRoom.doorNorth.SetActive(false);//if first row, open all north doors
                else if (i == rows - 1) tempRoom.doorSouth.SetActive(false);//if last row, open all south doors
                else {//if any other row, open both north and south doors
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }
                if (j == 0) tempRoom.doorEast.SetActive(false);//if first column, open all east doors
                else if (j == cols - 1) tempRoom.doorWest.SetActive(false);//if last column, open all west doors
                else {//if any other column, open both east and west doors
                    tempRoom.doorEast.SetActive(false);
                    tempRoom.doorWest.SetActive(false);
                }
                GameManager.instance.grid[j, i] = tempRoom;//place the room in the grid
            }
        }
    }
    public GameObject RandomRoomPrefab() { return gridPrefabs[UnityEngine.Random.Range(0, gridPrefabs.Length)]; }//returns a random room from the array
    public int DateToInt(DateTime d) { return d.Year + d.Month + d.Day + d.Hour + d.Minute + d.Second + d.Millisecond; }//converts date to int
}