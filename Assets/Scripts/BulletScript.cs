using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
    
    public TankMotor firedFrom;

    public float timeout = 5.0f;
    private float nextTimeout;

    void Start() {
        nextTimeout = Time.time + timeout;
    }
    void Update() {
        if (Time.time >= nextTimeout) Destroy(gameObject);
    }
    void OnCollisionEnter(Collision col) {
        Destroy(gameObject);
    }
}