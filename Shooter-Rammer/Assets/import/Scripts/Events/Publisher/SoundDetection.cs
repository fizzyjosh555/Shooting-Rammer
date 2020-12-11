using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            EventManagerDel.TriggerEvent("SoundHeard");
            Debug.Log("Sound detected");

        }
        else
        {

        }
        
    }
}
