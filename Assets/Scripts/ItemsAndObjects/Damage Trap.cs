using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DamageTrap : MonoBehaviour {

    public int timer;
    private bool isactivated;
    private int currentHealth;
    private Health player;
    public int healthDeBuffAmount = 5;

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
            currentHealth = player.currentHealth;
            player.currentHealth -= healthDeBuffAmount;
            StartCoroutine("playerHealthDeBuffTimer");
        }
    }

    IEnumerator playerHealthDeBuffTimer()
    {
        yield return new WaitForSeconds(timer);
        playerHealthDeBuffUndo();
    }

    void playerHealthDeBuffUndo()
    {

    }

}
