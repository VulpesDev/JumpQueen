using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    Vector3 currentPos, lastPos;
    [SerializeField] float stepDist;
    void Start()
    {
        currentPos = transform.position;
        lastPos = currentPos;
    }
    void Update()
    {
        currentPos = transform.position;
        if(Mathf.Abs(currentPos.magnitude - lastPos.magnitude) > stepDist)
        {
            lastPos = currentPos;
            //step
        }
    }
}
