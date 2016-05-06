using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class Trap : MonoBehaviour {

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
            player.m_WalkSpeed = 2;
            StartCoroutine("playerSpeedDebuffTimer");
        }
        Destroy(gameObject);
    }

    IEnumerator playerSpeedDebuffTimer()
    {
        yield return new WaitForSeconds(timer);
        playerSpeedDebuffUndo();
    }

    void playerSpeedDebuffUndo()
    {
        player.m_WalkSpeed = originalSpeed;
    }


}
