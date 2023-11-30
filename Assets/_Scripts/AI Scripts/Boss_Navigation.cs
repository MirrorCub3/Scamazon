using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Navigation : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    private int pIndex;
    private NavMeshAgent agent;
    [SerializeField] private bool moving;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pIndex = 0;
        moving = false;
        Count_Manager.bossAppears += startMoving;
    }

    private void Update()
    {
        if (moving) {
            if (!agent.pathPending) {
                if (agent.remainingDistance <= agent.stoppingDistance) {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                        if (pIndex >= 0 && pIndex < points.Length) {
                            move(points[pIndex].position);
                            pIndex++;
                        } else {
                            moving = false;
                            pIndex = 0;
                        }
                    }
                }
            }
        }

    }


    private void move(Vector3 loc)
    {
        agent.SetDestination(loc);
    }

    private void startMoving()
    {
        moving = true;
    }

}
