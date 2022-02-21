using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAwayFromPlayer : MonoBehaviour
{
    GameObject player;
    float dif;
    float ppos;
    [SerializeField] float speed = 0.8f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dif = Mathf.Abs(player.transform.position.z - transform.position.z);
    }
    void FixedUpdate()
    {
        ppos = player.transform.position.z;
        //transform.position = 
        //    new Vector3(transform.position.x, transform.position.y, ppos + dif);
        transform.position = Vector3.Lerp(transform.position,
            new Vector3(transform.position.x, transform.position.y, ppos + dif),
             speed * Time.fixedDeltaTime);
    }
}
