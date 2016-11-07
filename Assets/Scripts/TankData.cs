using UnityEngine;
using System.Collections;

public class TankData : MonoBehaviour {

    public float moveSpeed = 8.0f;//meters per second
    public float backwardsSpeed = 3.0f;//meters per second
    public float turnSpeed = 180.0f;//degrees per second

    public float bulletForce = 500.0f;//meters per second
    public float fireRate = 2.0f;//seconds

    public int lives = 3;
	public int prevLives = 3;
    public int maxLives = 3;

    public float health = 100.0f;//health the tank has
    public float maxHealth = 100.0f;//max health for tank
    public float damage = 20.0f;//damage the tank does

    public int score = 0;//score of the player
    public int scoreValue = 10;//points given for destroying player
}
