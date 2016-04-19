using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{

    public int maxHealth;

    [SyncVar][HideInInspector]
    public int currentHealth;

    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if(!isServer)
            return;

        currentHealth -= amount;

        // Death
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;

            // called on the server, will be invoked on the clients
            RpcRespawnZero();
        }
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
