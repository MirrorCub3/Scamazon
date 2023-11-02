using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    //[SerializeField] private float moveUnits;

    //orignal positions of the gameobject 
    //private Vector3 targetPos;
    //private Vector3 startPosition;

    //universal time variable to prevent jerky movement with MovePlatform()
    private float time;
    public void MoveObject(Transform current, Transform target, float duration)
    { 
        StartCoroutine(IMoveObject(current, target, duration));
    }


    private IEnumerator IMoveObject(Transform current, Transform target, float duration)
    {
        time = 0;
        while (time < duration)
        {
            current.position = Vector3.Lerp(current.position, target.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
