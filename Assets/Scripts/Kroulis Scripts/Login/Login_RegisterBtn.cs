using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Login_RegisterBtn : MonoBehaviour {

    public GameObject login;
    public GameObject register;

	// Use this for initialization
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(delegate() { this.Action(); });
    }
	
	void Action()
    {
        register.SetActive(true);
        login.SetActive(false);
    }
}
