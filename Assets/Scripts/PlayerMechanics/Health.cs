using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{

    public int maxHealth;

    [SyncVar]
    public int currentHealth;

    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth;
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
            RpcRespawnZero();
            return true;
        }

        return false;
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

}
