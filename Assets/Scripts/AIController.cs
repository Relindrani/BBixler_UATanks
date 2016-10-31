using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TankData))]
public class AIController : MonoBehaviour {

    public enum Personality { AllTalk = 0, Guard = 1, PatrolAvoid = 2, Flee = 3 };
    public enum AIStates { Patrol = 0, ChaseAndFire = 1, Flee = 2, Rest = 3, HearPlayer = 4, LookForPlayer = 5, CantHearPlayer = 6, SeePlayer = 7 };
    public Personality personality = Personality.AllTalk;
    public AIStates aiState;

    public float stateEnterTime;
    public float hearingRadius = 10.0f;//hearing radius for ai
    public float avoidRadius = 5.0f;//distance to stay away while fleeing
    public float fieldOfview = 30.0f;
    public float restingHealRate = 10.0f;//hp per second

    //Attack Mode AI variables
    public enum AvoidanceStage { Default, Turn, Advance };
    public AvoidanceStage avoidanceStage;

    public Transform target;

    public float fleeDistance = 1.0f;
    public float avoidanceTime = 2.0f;
    public float exitTime;

    //Patrol AI variables
    public enum LoopType { Stop, Loop, PingPong };
    public LoopType loopType;

    public Transform[] waypoints;
    public float closeEnough = 1.0f;

    private int currentWaypoint = 0;
    private bool isPatrolForward = true;

    //Shooting variables
    private float nextFireTime;

    //General variables
    public TankMotor motor;
    public TankData data;

    private Transform tf;

    void Start() {
        if (data == null) data = gameObject.GetComponent<TankData>();
        if (motor == null) motor = gameObject.GetComponent<TankMotor>();
        target = GameObject.FindGameObjectWithTag("Player").transform;//set initial target to player for personalities that require it
        nextFireTime = Time.time;
        //set default behaviors
        if (personality == Personality.AllTalk) aiState = AIStates.ChaseAndFire;
        else if (personality == Personality.Guard) aiState = AIStates.Patrol;
        else if (personality == Personality.PatrolAvoid) aiState = AIStates.Patrol;
        else if (personality == Personality.Flee) aiState = AIStates.Flee;
        //set hearing radius trigger size
        GetComponent<SphereCollider>().radius = hearingRadius;
    }
    void Awake() {
        tf = gameObject.GetComponent<Transform>();
    }
    void Update() {
        if (aiState == AIStates.Patrol) {
            if (!motor.RotateTowards(waypoints[currentWaypoint].position, data.turnSpeed)) motor.Move(data.moveSpeed);//if rotate towards returns false then move to the waypoint we are facing
            if (Vector3.SqrMagnitude(waypoints[currentWaypoint].position - tf.position) < (closeEnough * closeEnough)) {//if we are close enough to the waypoint
                if (loopType == LoopType.Stop) {//stop at last waypoint
                    if (currentWaypoint < waypoints.Length - 1) currentWaypoint++;//if there is another waypoint, set current waypoint to the next one
                } else if (loopType == LoopType.Loop) {//go back to first waypoint after last waypoint
                    if (currentWaypoint < waypoints.Length - 1) currentWaypoint++;//if there is another waypoint, set current waypoint to the next one
                    else currentWaypoint = 0;//else go back to the first waypoint
                } else if (loopType == LoopType.PingPong) {//go backwards after last waypoint
                    if ((isPatrolForward) ? currentWaypoint < waypoints.Length - 1 : currentWaypoint > 0) currentWaypoint += (isPatrolForward) ? 1 : -1;//if going forward then add 1 to the current waypoint, if not add -1 (subtract 1)
                    else {
                        isPatrolForward = !isPatrolForward;//reverse bool
                        currentWaypoint += (isPatrolForward) ? 1 : -1;//same as above
                    }
                }
            }
            if (personality == Personality.PatrolAvoid) {
                if (Vector3.Distance(target.position, tf.position) <= avoidRadius) ChangeState(AIStates.Flee);
            }
        } else if (aiState == AIStates.ChaseAndFire) {
            if (avoidanceStage == AvoidanceStage.Default) {
                Chase(target.position);
                if (Time.time >= nextFireTime) {
                    motor.Shoot(data.bulletForce);
                    nextFireTime = Time.time + data.fireRate;
                }
            } else Avoid();
            if (personality == Personality.AllTalk) CheckFlee();
        } else if (aiState == AIStates.Flee) {
            Vector3 vectorAwayFromTarget = -1 * (target.position - tf.position);
            vectorAwayFromTarget.Normalize();
            vectorAwayFromTarget *= fleeDistance;

            Vector3 fleePosition = vectorAwayFromTarget + tf.position;
            if (avoidanceStage == AvoidanceStage.Default) Chase(fleePosition);//"chase" opposite of target position
            else Avoid();//avoid obstacles
            if (Vector3.Distance(target.position, tf.position) > avoidRadius) {
                if (personality == Personality.AllTalk) ChangeState(AIStates.Rest);//if out of range while fleeing then rest
                else if (personality == Personality.PatrolAvoid) ChangeState(AIStates.Patrol);
            }
        } else if (aiState == AIStates.Rest) {
            Rest();//restore health

            if (!CheckFlee()) {//if it doesn't need to flee
                ChangeState(AIStates.ChaseAndFire);//chase and fire
            } else {//otherwise it changes state to flee
                if (Vector3.Distance(target.position, tf.position) > avoidRadius) ChangeState(AIStates.Rest);//but if we are out of fleeing range, rest more instead
            }
        } else if (aiState == AIStates.HearPlayer) {
            //TODO: Do something to alert that the player was heard
            ChangeState(AIStates.LookForPlayer);
        } else if (aiState == AIStates.LookForPlayer) {
            motor.RotateTowards(target.position, data.turnSpeed);
            CanSee();
            //TODO: something with FOV for sight
        } else if (aiState == AIStates.CantHearPlayer) {
            //TODO: player no longer within collider
            ChangeState(AIStates.Patrol);
        } else if (aiState == AIStates.SeePlayer) {
            //TODO: Do something to alert that the player was seen
            ChangeState(AIStates.ChaseAndFire);
        }
    }

    void Chase(Vector3 tar) {
        motor.RotateTowards(tar, data.turnSpeed);//turn towards target
        if (CanMove(data.moveSpeed)) motor.Move(data.moveSpeed);//if can move, then move
        else avoidanceStage = AvoidanceStage.Turn;//otherwise start avoidance
    }
    void Avoid() {
        if (avoidanceStage == AvoidanceStage.Turn) {//if turning
            motor.Rotate(-1 * data.turnSpeed);//turn to the left
            if (CanMove(data.moveSpeed * Time.deltaTime)) {//if can move at current rotation
                avoidanceStage = AvoidanceStage.Advance;//start advancing
                exitTime = avoidanceTime;//set time to stay in advancing stage
            }
        }else if (avoidanceStage == AvoidanceStage.Advance) {//if advancing
            if (CanMove(data.moveSpeed)) {//if can move
                exitTime -= Time.deltaTime;//update time
                motor.Move(data.moveSpeed);//move forward

                if (exitTime <= 0) avoidanceStage = AvoidanceStage.Default;//if time is up, go back to default avoidance
            }else avoidanceStage = AvoidanceStage.Turn;//if can't move, go back to turning
        }
    }
    bool CheckFlee() {
        if (data.health < data.maxHealth * 0.5f) {
            ChangeState(AIStates.Flee);
            return true;
        }return false;
    }
    void CanSee() {
        Vector3 targetPosition = target.transform.position;
        Vector3 vectorToTarget = targetPosition - tf.position;

        float angleToTarget = Vector3.Angle(vectorToTarget, tf.forward);

        if (angleToTarget < fieldOfview) {
            ChangeState(AIStates.SeePlayer);
        }
    }
    public void Rest() {
        data.health += restingHealRate * Time.deltaTime;//regen health based on time
        data.health = Mathf.Min(data.health, data.maxHealth);//clamp to max health if above
    }
    bool CanMove(float speed) {
        RaycastHit hit;
        return (Physics.Raycast(tf.position, tf.forward, out hit, speed)) ? (aiState == AIStates.Flee) ? false : (hit.collider.CompareTag("Player")) ? true : false : true;//if the raycast hit something and player is fleeing, then anything the raycast hit should be avoided. if the player is not fleeing and the raycast hit the player, then it can move towards the player. if it did not hit the player, it cannot move. if it didn't hit anything, it can move.
    }
    public void ChangeState(AIStates newState) {
        aiState = newState;
        stateEnterTime = Time.time;
    }
    
    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player") {
            target = col.transform;
            if(personality==Personality.Guard)
                ChangeState(AIStates.HearPlayer);
        }
    }
    void OnTriggerExit(Collider col) {
        if (col.tag == "Player") {
            if (personality == Personality.Guard)
                ChangeState(AIStates.CantHearPlayer);
        }
    }
}