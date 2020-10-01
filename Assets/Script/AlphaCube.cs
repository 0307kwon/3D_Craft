using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaCube : MonoBehaviour {

    public bool apply = true;

    private int timer = 1000;

    private List<Collision> target = new List<Collision>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name.Split('_')[0] == "Cube")
        {
            target.Add(collision);
            print(collision.transform.position);
            apply = false;
            transform.position = new Vector3(-10000000000, -10000000000000, 1);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.transform.name.Split('_')[0] == "Cube")
        {
            apply = true;
        }
              
    }

    private void Update()
    {
        timer--;
        if(timer < 0)
        {
            timer = 1000;
            target.Clear();
        }
    }

    public bool SetPosition(Vector3 a)
    {
        if (target != null)
        {
            foreach(Collision b in target)
            { 
                if (b.transform.position == a)
                {
                    print(b.transform.position + ", " + a);
                    return false;
                }
            }
            transform.position = a;
            return true;
            
        }
        else
        {
            transform.position = a;
            return true;
        }
    }
}
