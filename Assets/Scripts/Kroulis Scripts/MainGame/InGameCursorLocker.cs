using UnityEngine;
using System.Collections;


namespace Kroulis.Components
{
    public class InGameCursorLocker : MonoBehaviour
    {

        public bool LockMouse=false;
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if(LockMouse)
            {
                if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                    
            }
        }
    }
}