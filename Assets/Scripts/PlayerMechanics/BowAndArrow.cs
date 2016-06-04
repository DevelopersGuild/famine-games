using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Kroulis.UI.MainGame;

public class BowAndArrow : NetworkBehaviour
{
    public Arrow arrowPrefab;
    public float arrowSpeed = 100.0f;
    public bool fireOnArrow = false;
    public float fireRate = 1.5f;
    public float nextFire = 0.0f;
    [SyncVar]
    public float pullStartTime = 0.0f;
    [SyncVar]
    public float pullTime = 0.5f;
    [SyncVar]
    public bool falsePull;
    [SyncVar]
    public int currentAmmo;
    public float maxStrengthPullTime = 1.5f; // how long to hold button until max strength reached
    public bool bowEquipped;

    private AttackController ac;

    public void Start()
    {
        falsePull = false;
        bowEquipped = false;
        ac = GetComponent<AttackController>();
        currentAmmo = 0;
    }

    public void Update()
    {

        if (ac.currentWeapon == null)
            return;
        if (!isLocalPlayer)
            return;
        if (!GetComponent<NetworkedPlayer>().fpsController.GetInput())
            return;
        WeaponBarControl wbc = GameObject.Find("Main_UI").GetComponentInChildren<WeaponBarControl>();
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
            {
                pullStartTime = Time.time; //store the start time
                if (wbc && currentAmmo>0)
                {
                    wbc.StartCharge(maxStrengthPullTime);
                }
            }   
            else
                falsePull = true;

        }

        
        

        // fire arrow
        if (Input.GetMouseButtonUp(0) && currentAmmo > 0)
        {
            //your way wouldn't work right, since you just increased nextFire
            if (!falsePull)
            {
                nextFire = Time.time + pullTime; // this is the actual fire rate as things stand now

                float timePulledBack = Time.time - pullStartTime; // this is how long the button was held
                if (timePulledBack > maxStrengthPullTime) // this says max strength is reached 
                    timePulledBack = maxStrengthPullTime; // max strength is ArrowSpeed * maxStrengthPullTime

                float currentArrowSpeed = arrowSpeed*timePulledBack; // adjust speed directly using pullback time
                if (wbc)
                {
                    wbc.StopCharge();
                }
                Arrow arrow =
                    (Arrow)
                        Instantiate(arrowPrefab, transform.position - transform.forward*-2,
                            Camera.main.transform.rotation);
                if (fireOnArrow)
                {
                    
                }
                arrow.parentNetId = netId;
                arrow.SetDamage(ac.currentWeapon.damage);
                Destroy(arrow.gameObject, 10);


                Physics.IgnoreCollision(arrow.GetComponent<Collider>(), transform.root.GetComponent<Collider>());
                Physics.IgnoreCollision(arrow.GetComponent<Collider>(), ac.GetComponent<Collider>());

                arrow.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward*currentArrowSpeed);
                // adjusted speed
                NetworkServer.Spawn(arrow.gameObject);
                currentAmmo--;
            }
            else
                falsePull = false;
        }

        if (Input.GetKeyDown("f"))
        {
            AddfireToArrow();
        }
    }

    void AddfireToArrow()
    {
        fireOnArrow = true;
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

    public void pickupAmmo(int amount)
    {
        currentAmmo += amount;
    }
}
