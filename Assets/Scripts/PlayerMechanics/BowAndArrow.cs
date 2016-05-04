using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BowAndArrow : NetworkBehaviour
{
    public AttackCollider arrowPrefab;
    public float arrowSpeed = 100.0f;
    public float fireRate = 1.5f;
    public float nextFire = 0.0f;
    public float pullStartTime = 0.0f;
    public float pullTime = 0.5f;
    public bool falsePull;
    public float maxStrengthPullTime = 1.5f; // how long to hold button until max strength reached

    public void Start()
    {
        falsePull = false;
    }

    public void Update()
    {
        if (!isLocalPlayer)
            return;

        // pull back string
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate; // this line is unnecessary, since you are going to change it onMouseUp
                pullStartTime = Time.time; //store the start time
            }
            else
                falsePull = true;
        }

        // fire arrow
        if (Input.GetMouseButtonUp(0))
        {
            //your way wouldn't work right, since you just increased nextFire
            if (!falsePull)
            {
                CmdShoot();
            }
            else
                falsePull = false;
        }
    }

    [Command]
    public void CmdShoot()
    {
        nextFire = Time.time + pullTime; // this is the actual fire rate as things stand now

        float timePulledBack = Time.time - pullStartTime; // this is how long the button was held
        if (timePulledBack > maxStrengthPullTime) // this says max strength is reached 
            timePulledBack = maxStrengthPullTime; // max strength is ArrowSpeed * maxStrengthPullTime
        float currentArrowSpeed = arrowSpeed * timePulledBack; // adjust speed directly using pullback time

        AttackCollider arrow = (AttackCollider)Instantiate(arrowPrefab, transform.position - transform.forward * -2, transform.rotation);


        Physics.IgnoreCollision(arrowPrefab.GetComponent<Collider>(), transform.root.GetComponent<Collider>());

        arrow.GetComponent<Rigidbody>().AddForce(transform.forward * currentArrowSpeed); // adjusted speed
        NetworkServer.Spawn(arrow.gameObject);
    }
}
