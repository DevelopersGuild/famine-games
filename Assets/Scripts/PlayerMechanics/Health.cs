using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{

    public int maxHealth;
    public int bandagesAmount;
    private NetworkStartPosition[] spawnPoints = new NetworkStartPosition[8];

    [SyncVar]
    public int currentHealth;

    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth;
        bandagesAmount = 0;
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
    }

    // Returns whether the target died
    public bool TakeDamage(int amount)
    {
        if(!isServer)
            return false;

        currentHealth -= amount;

        // Death
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
            // called on the server, will be invoked on the clients
            GetComponent<Defense>().CmdDeadAmrorBreak();
            GetComponent<AttackController>().CmdDeadWeaponDrop();
            RpcRespawnRandom();
            return true;
        }

        return false;
    }

    public void Heal(int amount)
    {
        if (!isServer)
            return;

        currentHealth += amount;

        // Health cap
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void UseBandage(int amount)
    {
        if(bandagesAmount > 0)
        {
            bandagesAmount--;
            Heal(amount);
        }
    }

    public void pickupBandage()
    {
        bandagesAmount++;
    }

    [ClientRpc]
    void RpcRespawn(Vector3 position)
    {
        if (isLocalPlayer)
        {
            transform.position = position;
        }
    }

    [ClientRpc]
    void RpcRespawnZero()
    {
        if (isLocalPlayer)
        {
            // move back to zero location
            transform.position = Vector3.zero;
        }
    }

    [ClientRpc]
    void RpcRespawnRandom()
    {
        if (isLocalPlayer)
        {
            // move back to zero location
            int randomInt = (int)(Mathf.Floor(Random.Range(0, spawnPoints.GetLength(0))));
            transform.position = spawnPoints[randomInt].transform.position;
        }
    }


}
