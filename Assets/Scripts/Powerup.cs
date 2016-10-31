using UnityEngine;
using System.Collections;

[System.Serializable]
public class Powerup {
    public float speedModifier;
    public float healthModifier;
    public float maxHealthModifier;
    public float damageModifier;
    public float fireRateModifier;

    public float duration;

    public bool isPermanent;

    public void OnActivate(TankData target) {
        target.moveSpeed += speedModifier;
        target.health += healthModifier;
        target.maxHealth += maxHealthModifier;
        if (target.health > target.maxHealth) target.health = target.maxHealth;//clamp health
        target.damage += damageModifier;
        target.fireRate -= fireRateModifier;
    }
    public void OnDeactivate(TankData target) {
        target.moveSpeed -= speedModifier;
        target.health -= healthModifier;
        target.maxHealth -= maxHealthModifier;
        target.damage -= damageModifier;
        target.fireRate += fireRateModifier;
    }
}