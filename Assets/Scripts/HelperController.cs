using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperController : MonoBehaviour
{

    public GameObject player;
    public float targetDistance;
    public float allowedDistance = 3f;
    public float speed;
    public RaycastHit hit;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        transform.LookAt(player.transform);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            targetDistance = hit.distance;
            if (targetDistance >= allowedDistance)
            {
                speed = 0.03f;
                anim.Play("walk");
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
            }
            else
            {
               speed = 0f;
            }
        }
    }
}
