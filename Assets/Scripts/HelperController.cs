using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperController : MonoBehaviour
{
    //State for our Finite State Machine
    enum State
    {
        follow,
        pickup
    };

    public GameObject player;
    public float targetDistance;
    public float allowedDistance = 3f;
    public float speed;
    public float lookRadius = 5f;
    public RaycastHit hit;

    private Animator anim;
    private State currentState = State.follow;
    private Collider[] objectsAround;
    private GameObject currentPickup;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            targetDistance = hit.distance;
            if (targetDistance <= allowedDistance)
            {
                LookAround();
            }
        }
        
        switch(currentState)
        {
            case State.follow:
                FollowUpdate();
                break;
            case State.pickup:
                PickupUpdate();
                break;
        }
    }

    void FollowUpdate()
    {
        transform.LookAt(player.transform);

        if (Physics.Raycast( transform.position, transform.TransformDirection(Vector3.forward), out hit) )
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

    void PickupUpdate()
    {
        transform.LookAt(currentPickup.transform);
        speed = 0.03f;
        anim.Play("walk");
        transform.position = Vector3.MoveTowards(transform.position, currentPickup.transform.position, speed);
    }

    void LookAround()
    {
        objectsAround = Physics.OverlapSphere(GetComponent<Transform>().position, lookRadius);

        for (int i = 0; i < objectsAround.Length; ++i)
        {
            if (objectsAround[i].gameObject.CompareTag("pickup"))
            {
                Debug.Log("Found object!!");
                currentState = State.pickup;
                currentPickup = objectsAround[i].gameObject;
                break;
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("pickup"))
        {
            Destroy(col.gameObject);
            currentState = State.follow;
        }
    }

}
