using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker_Navigation : MonoBehaviour
{
    [SerializeField] private List<Transform> exits;
    [SerializeField] private float radius = 12;
    [SerializeField] private Vector2 waitRange;
    [SerializeField] private float turnDuration;
    private NavMeshAgent agent;
    private bool turning;
    private bool moving;

    void Start()
    {
        moving = false;
        turning = false;
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(wait());
    }

    private void Update()
    {
        if(moving && !turning && Vector3.Distance(agent.destination, transform.position) < agent.stoppingDistance) {
            turning = true;
            moving = false;
            StartCoroutine(turn());
        }
    }

    private IEnumerator wait()
    {
        //print("waiting");
        int waitTime = Random.Range((int)(waitRange.x), (int)(waitRange.y));
        yield return new WaitForSeconds(waitTime);
        wander();
    }

    private void wander()
    {
        //print("wandering");
        Vector3 finalPosition = Vector3.zero;

        while (finalPosition == Vector3.zero) {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
                finalPosition = hit.position;
            }
        }
        
        moving = true;
        agent.SetDestination(finalPosition);
    }


    private IEnumerator turn()
    {
        //print("turning");
        Quaternion endValue = Quaternion.Euler(0, Random.Range(0, 360), 0);
        float time = 0;
        Quaternion startValue = transform.rotation;
        while (time < turnDuration) {
            transform.rotation = Quaternion.Lerp(startValue, endValue, time / turnDuration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endValue;

        turning = false;
        goToExit();
    }

    private void goToExit()
    {
        int index = Random.Range(0, exits.Count);
        agent.SetDestination(exits[index].position);
        StartCoroutine(wait());
    }

}
