using UnityEngine;
using System.Collections;

public class TankData : MonoBehaviour {

    public float moveSpeed = 8.0f;//meters per second
    public float backwardsSpeed = 3.0f;//meters per second
    public float turnSpeed = 180.0f;//degrees per second

    public float bulletForce = 500.0f;//meters per second
    public float fireRate = 2.0f;//seconds

    public float health = 100.0f;//health the tank has
    public float maxHealth = 100.0f;//max health for tank
    public float damage = 20.0f;//damage the tank does

    public float score = 0.0f;//score of the player
    public float scoreValue = 10.0f;//points given for destroying player
}
