using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DamageTrap : MonoBehaviour {

    public int timer;
    private bool isactivated;
    private int currentHealth;
    private Health player;
    public int healthDamageAmount = 10;

    // Use this for initialization
    void Start () {
        isactivated = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<Health>();
        if (player != null)
        {
            player.TakeDamage(healthDamageAmount);
            StartCoroutine("playerHealthDamageTimer");
        }

        Destroy(gameObject);
    }

    IEnumerator playerHealthDamageTimer()
    {
        yield return new WaitForSeconds(timer);
      
    }

    
}
