using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    private bool local = false;
    // Use this for initialization
    void Start()
    {
        if (transform.parent.GetComponent<PlayerController>().isLocalPlayer)
        {
            local = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (local) { 
            if (other.name.Split('_')[0] == "Cube")
            {
                transform.parent.GetComponent<PlayerController>().CanJump(true);
            }
        }
    }
}
