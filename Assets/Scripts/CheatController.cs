using UnityEngine;
using System.Collections;

public class CheatController : MonoBehaviour {

    public PowerupController powCon;
    public Powerup cheatPowerup;
    
	void Start () {
        if (powCon == null) powCon = gameObject.GetComponent<PowerupController>();
	}
	void Update () {
        if (Input.GetKey(KeyCode.H) && Input.GetKey(KeyCode.U) && Input.GetKeyDown(KeyCode.E)) powCon.Add(cheatPowerup);
	}
}