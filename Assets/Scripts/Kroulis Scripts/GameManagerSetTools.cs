using UnityEngine;
using System.Collections;
using Kroulis.UI.Launcher;

public class GameManagerSetTools : MonoBehaviour {


    public bool testing=false;
    public bool load_from_internet = true;
    public string uid="";
    public string character_name = "";
    public bool show_connection_gui=false;
    private GameObject Launcher_UI_Root;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
        if(testing==true)
        {
            if(load_from_internet==false)
            {
                Launcher_UI_Root = GameObject.Find("Launcher UI Root");
                if(Launcher_UI_Root)
                {
                    Launcher_UI_Root.GetComponent<UI_FunctionControl>().IndexPanel.GetComponent<IndexPanel_FullControl>().name.text = character_name;
                    Launcher_UI_Root.GetComponent<UI_FunctionControl>().FinishWWWLoading();
                }
                else
                {
                    Debug.LogWarning("Failed to find the launcher ui object");
                }
            }
            else
            {
                Globe.uid = uid;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
