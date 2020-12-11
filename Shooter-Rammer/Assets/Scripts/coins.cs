using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coins : MonoBehaviour
{
    public coincoutscript b;
    public GameObject self;
    // Start is called before the first frame update
    void Start()
    {
        self = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            b.countup();
            self.SetActive(false);
        }
        else
        {

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (self.activeSelf)
            {
                Debug.Log("Deactivate Error");
            }
        }
        else
        {

        }
    }
}
