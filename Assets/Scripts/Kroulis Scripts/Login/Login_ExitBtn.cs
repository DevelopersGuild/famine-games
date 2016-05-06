using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Login_ExitBtn : MonoBehaviour {

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(delegate() { this.Action(); });
    }
    void Action()
    {
        Debug.Log("Game Exited.");
        Application.Quit();
    }
}
