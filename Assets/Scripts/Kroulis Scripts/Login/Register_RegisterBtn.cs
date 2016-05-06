using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Register_RegisterBtn : MonoBehaviour {

    public Text username;
    public Text password;
    public Text email;
    public Text character_name;
    public Text tips;
    public Logic_Cilent_Register reg;
	// Use this for initialization
	void Start () {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(delegate() { this.Action(); });
	}

    void Action()
    {
        if(username.text=="" || password.text=="" || email.text=="" || character_name.text=="")
        {
            tips.color = Color.red;
            tips.text = "Please input all the infomation above.";
            return;
        }
        reg.Register(username,password,email,character_name,tips);
    }
	
}
