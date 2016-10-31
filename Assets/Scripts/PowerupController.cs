using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupController : MonoBehaviour {

    public List<Powerup> powerups;

    public TankData data;
    
	void Start () {
        powerups = new List<Powerup>();
        if (data == null) data = gameObject.GetComponent<TankData>();
	}
    void Update () {
        List<Powerup> expiredPowerups = new List<Powerup>();
	    foreach(Powerup power in powerups) {
            power.duration -= Time.deltaTime;

            if (power.duration <= 0) {
                expiredPowerups.Add(power);
            }
        }
        foreach(Powerup power in expiredPowerups) {
            power.OnDeactivate(data);
            powerups.Remove(power);
        }
        expiredPowerups.Clear();
	}
    
    public void Add(Powerup powerup) {
        powerup.OnActivate(data);
        if(!powerup.isPermanent) powerups.Add(powerup);
    }
}