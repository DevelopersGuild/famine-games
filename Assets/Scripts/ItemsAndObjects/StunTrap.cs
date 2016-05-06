using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class StunTrap : MonoBehaviour {

    public int timer;
    private bool isactivated;
    private float originalSpeed;
    private FirstPersonController player;

    // Use this for initialization
    void Start () {
        isactivated = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<FirstPersonController>();
        if (player != null)
        {
            originalSpeed = player.m_WalkSpeed;
            Debug.Log("oriS: " + originalSpeed.ToString());
            player.m_WalkSpeed = 0;
            StartCoroutine("playerStunTimer");
        }

    }

    IEnumerator playerStunTimer()
    {
        yield return new WaitForSeconds(timer);
        playerStunUndo();
    }

    void playerStunUndo()
    {
        player.m_WalkSpeed = originalSpeed;
        Destroy(gameObject);
        Debug.Log("PlayerS: " + player.m_WalkSpeed.ToString());
    }


}
