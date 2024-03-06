using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{

    public Transform savedPath;
    public float speed = 5;
    public float turnSpeed = 3;
    public float waitTime = 0.1f;

    public Light spotLight;
    public float viewDistance;
    float viewAngle;
    Transform player;
    public LayerMask viewMask;
    Color orgSpotLightColour;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotLight.spotAngle;
        orgSpotLightColour = spotLight.color;

        viewAngle = spotLight.spotAngle;
        Vector3[] waypoints = new Vector3[savedPath.childCount];
        for (int i=0; i<waypoints.Length; i++){
            waypoints[i] = savedPath.GetChild(i).position;
            waypoints[i].y = transform.position.y;
        }
        StartCoroutine(patrolPath(waypoints));
    }

    void Update() {
        if (CanSeePlayer()) {
            spotLight.color = Color.red;
        }
        else{
            spotLight.color = orgSpotLightColour;
        }
    }

    bool CanSeePlayer(){
        if (Vector3.Distance(transform.position, player.position)< viewDistance){
            Vector3 dirToPlayer = (player.position -transform.position).normalized;
            float angleBetweenGuardandPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardandPlayer < viewAngle /2f) {
                if (!Physics.Linecast(transform.position, player.position, viewMask)){
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator patrolPath(Vector3[] waypoints) {

        transform.position = waypoints[0];

        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);


        while (true) {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed*Time.deltaTime);
            if (transform.position == targetWaypoint) {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(rotateToPoint(targetWaypoint));
            }
            yield return null;
        }
    }

    IEnumerator rotateToPoint(Vector3 lookTarget) {

        Vector3 dirToLook = (lookTarget-transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLook.z, dirToLook.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle))>0.05){
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    void OnDrawGizmos(){
        
        Vector3 startPosition = savedPath.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach(Transform waypoint in savedPath){
            Gizmos.DrawSphere (waypoint.position, 0.3f);
            Gizmos.DrawLine (previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition,startPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawRay (transform.position, transform.forward * viewDistance);
    }
}
