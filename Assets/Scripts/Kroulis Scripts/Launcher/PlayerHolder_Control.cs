using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kroulis.UI.Launcher
{
    public class PlayerHolder_Control : MonoBehaviour
    {

        private RawImage ri;
        private Texture[] t;
        private float timer;
        // Use this for initialization
        void Start()
        {
            ri = GetComponent<RawImage>();
            t = Resources.LoadAll<Texture>("PlayerHolder");
            //Debug.Log(t.Length);
            timer = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            timer = (timer + Time.deltaTime) % 2f;
            ri.texture = t[(int)(timer * 30)];
        }
    }
}
