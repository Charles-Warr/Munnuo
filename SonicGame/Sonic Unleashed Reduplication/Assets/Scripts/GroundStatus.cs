using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundStatus : MonoBehaviour
{
    public bool ground;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            ground = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Ground")
        {
            ground = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            ground = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
