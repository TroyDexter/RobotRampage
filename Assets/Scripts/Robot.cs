using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField] private string robotType;

    public int health;
    public int range;
    public float fireRate;

    public Transform missileFireSpot;
    private UnityEngine.AI.NavMeshAgent agent;

    private Transform player;
    private float timeLastFired;

    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the robot is dead before continuing
        if (isDead)
        {
            return;
        }

        //Make the robot face the player
        transform.LookAt(player);

        //Tell the robot to use the NavMesh to find the player
        agent.SetDestination(player.position);

        //Check to see if the robot is within firing range and if there’s been enough time between shots to fire again.
        if (Vector3.Distance(transform.position, player.position) < range && Time.time - timeLastFired > fireRate)
        {
            //Update timeLastFired to the current time
            timeLastFired = Time.time;
            fire();
        }
    }

    private void fire()
    {
        Debug.Log("Fire");
    }
}
