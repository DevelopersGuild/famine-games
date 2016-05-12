using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BowAndArrow : NetworkBehaviour
{
    public Arrow arrowPrefab;
    public float arrowSpeed = 100.0f;
    public float fireRate = 1.5f;
    public float nextFire = 0.0f;
    [SyncVar]
    public float pullStartTime = 0.0f;
    [SyncVar]
    public float pullTime = 0.5f;
    [SyncVar]
    public bool falsePull;
    public float maxStrengthPullTime = 1.5f; // how long to hold button until max strength reached
    public bool bowEquipped;

    private AttackController ac;

    public void Start()
    {
        falsePull = false;
        bowEquipped = false;
        ac = GetComponent<AttackController>();
    }

    public void Update()
    {
        if (ac.currentWeapon == null)
            return;
        if (!isLocalPlayer)
            return;

        // Check for bow equipment
        if (ac.currentWeapon.currentWeaponType == Weapon.WeaponType.Ranged)
            bowEquipped = true;
        else
            bowEquipped = false;
        

        // pull back string
        if (!bowEquipped) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time > nextFire)
                pullStartTime = Time.time; //store the start time
            else
                falsePull = true;
        }

        // fire arrow
        if (Input.GetMouseButtonUp(0))
        {
            //your way wouldn't work right, since you just increased nextFire
            if (!falsePull)
            {
                nextFire = Time.time + pullTime; // this is the actual fire rate as things stand now

                float timePulledBack = Time.time - pullStartTime; // this is how long the button was held
                if (timePulledBack > maxStrengthPullTime) // this says max strength is reached 
                    timePulledBack = maxStrengthPullTime; // max strength is ArrowSpeed * maxStrengthPullTime
                float currentArrowSpeed = arrowSpeed*timePulledBack; // adjust speed directly using pullback time

                Arrow arrow =
                    (Arrow)
                        Instantiate(arrowPrefab, transform.position - transform.forward*-2,
                            Camera.main.transform.rotation);
                arrow.parentNetId = netId;
                arrow.SetDamage(ac.currentWeapon.damage);
                Destroy(arrow.gameObject, 10);


                Physics.IgnoreCollision(arrow.GetComponent<Collider>(), transform.root.GetComponent<Collider>());

                arrow.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward*currentArrowSpeed);
                // adjusted speed
                NetworkServer.Spawn(arrow.gameObject);
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

        Arrow arrow = (Arrow)Instantiate(arrowPrefab, transform.position - transform.forward * -2, Camera.main.transform.rotation);
        arrow.parentNetId = netId;
        arrow.SetDamage(ac.currentWeapon.damage);
        Destroy(arrow.gameObject, 10);


        Physics.IgnoreCollision(arrow.GetComponent<Collider>(), transform.root.GetComponent<Collider>());

        arrow.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * currentArrowSpeed); // adjusted speed
        NetworkServer.Spawn(arrow.gameObject);
    }
}
