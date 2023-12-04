using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Manager_Navigation : MonoBehaviour
{
    [SerializeField] private Transform homeBase;
    [SerializeField] private Transform player;

    private NavMeshAgent agent;
    private static GameObject target;
    private static Vector3 location;

    private bool turning;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = null;
        turning = false;
    }

    private void Update()
    {
        if (target) {
            agent.SetDestination(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));

            if (Vector3.Distance(agent.destination, transform.position) < agent.stoppingDistance) {
                if (target.activeSelf && target.transform.position == location) {

                    Vector3 lookPos = player.position - transform.position;
                    lookPos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(lookPos);
                    StartCoroutine(turn(rotation, 0.5f));

                    target = null;
                    print("No littering!"); // PLAY SOUND HERE 
                }
            }
        } else {
            if (!turning) {
                agent.SetDestination(homeBase.position);
                if (Vector3.Distance(agent.destination, transform.position) < agent.stoppingDistance && transform.rotation != Quaternion.Euler(0, -90, 0)) {
                    StartCoroutine(turn(Quaternion.Euler(0, -90, 0), 1f));
                }
            }
            
        }
    }

    public static void setTarget(GameObject tg, Vector3 loc)
    {
        target = tg;
        location = loc;
    }

    public static void updateTargetLoc(GameObject tg, Vector3 loc)
    {
        if (tg == target)
            location = loc;
    }

    private IEnumerator turn(Quaternion endAngle, float duration)
    {
        turning = true;
        float time = 0;
        Quaternion startValue = transform.rotation;
        while (time < duration) {
            transform.rotation = Quaternion.Lerp(startValue, endAngle, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endAngle;
        if (endAngle != Quaternion.Euler(0, -90, 0))
            yield return new WaitForSeconds(1);
        turning = false;
    }

}
