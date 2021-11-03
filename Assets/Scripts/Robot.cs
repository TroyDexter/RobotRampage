using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private string robotType;

    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip weakHitSound;

public int health;
    public int range;
    public float fireRate;

    public Transform missileFireSpot;
    private UnityEngine.AI.NavMeshAgent agent;

    private Transform player;
    private float timeLastFired;

    private bool isDead;

    public Animator robot;

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
        GameObject missile = Instantiate(missilePrefab);
        missile.transform.position = missileFireSpot.transform.position;
        missile.transform.rotation = missileFireSpot.transform.rotation;
        robot.Play("Fire");
        GetComponent<AudioSource>().PlayOneShot(fireSound);
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }

        health -= amount;

        if (health <= 0)
        {
            isDead = true;
            robot.Play("Die");
            StartCoroutine("DestroyRobot");
            GetComponent<AudioSource>().PlayOneShot(deathSound);
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(weakHitSound);
        }
    }

    //2
    IEnumerator DestroyRobot()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

}
