﻿using UnityEngine;
using System.Collections;

public class TankMotor : MonoBehaviour {

    private CharacterController characterController;
    public GameObject bulletPrefab;

    public TankData data;

    public Transform tf;

    void Start () {
        //set characterController to the objects character controller
        characterController = gameObject.GetComponent<CharacterController>();
        if (data != null) data = gameObject.GetComponent<TankData>();
        data.health = data.maxHealth;
        data.moveSpeed = data.maxSpeed;
	}
    void Update () {

    }
    void Awake() {
        tf = gameObject.GetComponent<Transform>();
    }
    

    //call simple move function using the forward vector of the object multiplied by the speed for it to move
    public void Move(float speed) {
        characterController.SimpleMove(transform.forward * speed);
    }
    //Rotate object using the vector3.up for direction, speed for how far to rotate and time.delatetime to update based on real time and not based on frames. Space.self causes it to rotate in local space.
    public void Rotate(float speed) {
        tf.Rotate(Vector3.up * speed * Time.deltaTime, Space.Self);
    }
    //Returns true if function can (and did) rotate, false if it is already rotated in the right direction
    public bool RotateTowards(Vector3 target, float speed) {
        Vector3 vectorToTarget;//vector to target is difference between target pos and our pos
        vectorToTarget = target - tf.position;//subtract our pos from target pos
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget);//get the rotation of vector to target
        if (targetRotation == tf.rotation) return false;//return false if facing target since there is no need to rotate
        tf.rotation = Quaternion.RotateTowards(tf.rotation, targetRotation, data.turnSpeed * Time.deltaTime);//rotate towards target rotation over time using turn speed and deltatime
        return true;//return true because we did rotate
    }
    public void Shoot(float speed) {
        GameObject bullet;//create bullet gameobject
        bullet = Instantiate(bulletPrefab, tf.position + tf.forward * 2, Quaternion.identity) as GameObject;//instantiate bullet in the world
        bullet.GetComponent<BulletScript>().firedFrom = this;//define which tank motor fired the bullet
        bullet.GetComponent<Rigidbody>().AddForce(tf.forward * speed);//add force
    }

    void OnCollisionEnter(Collision col) {//if the tank collides with something
        if (col.collider.gameObject.tag == "Bullet") {//if it is a bullet
            if (col.gameObject.GetComponent<BulletScript>().firedFrom != this) {
                data.health -= col.gameObject.GetComponent<BulletScript>().firedFrom.data.damage;//subtract this tanks health by the damage of the tank that fired the bullet, if it wasn't fired from itself
            }
            if (data.health <= 0) {
                col.gameObject.GetComponent<BulletScript>().firedFrom.data.score += data.scoreValue;//add score when destroyed
                if (gameObject.tag == "Player") {
                    foreach(Transform child in transform) {
                        if (child.name == "Visuals") {
                            Destroy(child.gameObject);//destroy the visuals of the player tank without destroying the only camera in the scene
                        }
                    }
                    Time.timeScale = 0;//pause game
                    gameObject.GetComponent<InputController>().enabled = false;//disable input since tank is destroyed
                    GameManager.instance.GameOver();
                }else {
                    Destroy(gameObject);
                    GameManager.instance.remainingEnemies -= 1;
                    if (GameManager.instance.remainingEnemies <= 0) {
                        Time.timeScale = 0;
                        GameObject.FindGameObjectWithTag("Player").GetComponent<InputController>().enabled = false;//disable input since you won
                        GameManager.instance.Victory();
                    }
                }
            }
        }
    }
}