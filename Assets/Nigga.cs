using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nigga : MonoBehaviour
{
    public Transform targetTransform;
    float speed = 7;

    // Start is called before the first frame update
    // void Start()
    // {

    // }

    // Update is called once per frame

    void Update()
    {
        Vector3 displacementFromTarget = targetTransform.position - transform.position;
        Vector3 directionToTarget = displacementFromTarget.normalized;
        Vector3 velocity = directionToTarget * speed;
        float distanceToTarget = displacementFromTarget.magnitude;

        if (distanceToTarget > 0.9f)
        {
            transform.position += velocity * Time.deltaTime;
        }

    }
}
