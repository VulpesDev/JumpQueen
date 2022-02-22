using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAwayFromPlayer : MonoBehaviour
{
    GameObject player;
    float dif;
    float ppos;
    bool move = false;
    [SerializeField] float speed = 0.8f, pdistanceMin = 1.0f, pdistanceMax = 10.0f, pdistance = 0f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dif = Mathf.Abs(player.transform.position.z - transform.position.z);
    }
    void FixedUpdate()
    {
        ppos = player.transform.position.z;
        pdistance = (transform.position - player.transform.position).magnitude;
        if (pdistance <= pdistanceMin)
            move = true;
        else if (pdistance >= pdistanceMax)
            move = false;
        if (move)
        {
            transform.position = Vector3.Lerp(transform.position,
            new Vector3(transform.position.x, transform.position.y, ppos + dif),
             Mathf.Clamp(pdistance, 0, 1)* speed * Time.fixedDeltaTime);
        }
    }
}