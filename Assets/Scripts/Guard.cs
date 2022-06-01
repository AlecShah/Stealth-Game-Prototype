using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{

    public Transform pathway;
    public float speed = 5;
    public float waitTime = .3f;
    public float turnSpeed = 90f;

    public Light spotlight;
    public float viewDistance;
    private float viewAngle;

    private Transform smoke;
    public LayerMask viewMask;
    private Color originalSpotlightColor;

    Animator animate;

    private void Start()
    {

        animate = GetComponent<Animator>();
        smoke = GameObject.FindGameObjectWithTag("smoke").transform;
        viewAngle = spotlight.spotAngle;
        originalSpotlightColor = spotlight.color;

        Vector3[] waypoints = new Vector3[pathway.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathway.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

        StartCoroutine(FollowPath(waypoints));
    }


    private void Update()
    {
        if (CanSeeSmoke())
        {
            spotlight.color = Color.red;
        }
        else
        {
            spotlight.color = originalSpotlightColor;
        }
    }

    bool CanSeeSmoke()
    {
        if (Vector3.Distance(transform.position, smoke.position) < viewDistance)
        {
            Vector3 dirToSmoke = (smoke.position - transform.position).normalized;
            float angleBetweenGuardandSmoke = Vector3.Angle(transform.forward, dirToSmoke);
            if (angleBetweenGuardandSmoke < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, smoke.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);


        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            animate.SetFloat("Blend", 1, 0.25f, Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
                
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }

            
            yield return null;
        }

    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            animate.SetFloat("Blend", 0, 0.25f, Time.deltaTime);
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }

    }

    
    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathway.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathway)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }

        Gizmos.DrawLine(previousPosition, startPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
    


}
