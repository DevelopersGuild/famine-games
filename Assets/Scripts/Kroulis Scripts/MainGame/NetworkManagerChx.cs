using UnityEngine;
using System.Collections;

namespace Kroulis.MainGame
{
    public class NetworkManagerChx : MonoBehaviour
    {
        void Awake()
        {
            if(GameObject.Find("Logic_Network"))
            {
                Destroy(this.gameObject);
            }
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}