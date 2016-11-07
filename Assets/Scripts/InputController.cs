using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TankData))]
public class InputController : MonoBehaviour {

    public enum InputScheme { WASD, ArrowKeys };
    public InputScheme scheme = InputScheme.WASD;

    public TankMotor motor;
    public TankData data;

    private float nextFireTime;

	void Start() {
        if (data == null) data = gameObject.GetComponent<TankData>();
        if (motor == null) motor = gameObject.GetComponent<TankMotor>();
        nextFireTime = Time.time;
    }
    void Update() {
        switch (scheme) {
            case InputScheme.WASD:
                if (Input.GetKey(KeyCode.W)) motor.Move(data.moveSpeed);
                else if (Input.GetKey(KeyCode.S)) motor.Move(-data.backwardsSpeed);
                if (Input.GetKey(KeyCode.A)) motor.Rotate(-data.turnSpeed);
                else if (Input.GetKey(KeyCode.D)) motor.Rotate(data.turnSpeed);
				if (Input.GetKeyDown(KeyCode.Space) && Time.time>=nextFireTime) {
					motor.Shoot(data.bulletForce);
					nextFireTime = Time.time + data.fireRate;
				}
                break;
            case InputScheme.ArrowKeys:
                if(Input.GetKey(KeyCode.UpArrow)) motor.Move(data.moveSpeed);
                if (Input.GetKey(KeyCode.DownArrow)) motor.Move(-data.backwardsSpeed);
                if (Input.GetKey(KeyCode.LeftArrow)) motor.Rotate(-data.turnSpeed);
                if (Input.GetKey(KeyCode.RightArrow)) motor.Rotate(data.turnSpeed);
				if (Input.GetKeyDown(KeyCode.Keypad0) && Time.time>=nextFireTime) {
					motor.Shoot(data.bulletForce);
					nextFireTime = Time.time + data.fireRate;
				}
                break;
        }
        
    }
}