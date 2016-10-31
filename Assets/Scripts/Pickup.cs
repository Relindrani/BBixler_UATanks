using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    public Powerup powerup;
   
    public AudioClip feedback;

    private Transform tf;

    void Start () {
        
	}
	void Update () {
	
	}
    void Awake() {
        tf = gameObject.GetComponent<Transform>();
    }

    public void OnTriggerEnter(Collider other) {
        PowerupController powCon = other.GetComponent<PowerupController>();

        if (powCon != null) {
            powCon.Add(powerup);
            if (feedback != null) AudioSource.PlayClipAtPoint(feedback, tf.position, 1.0f);
            Destroy(gameObject);
        }
    }
}