using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerdetectwallcollission : MonoBehaviour
{
    Material m_Material;
    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        m_Material = GetComponent<Renderer>().material;
        m_Material.color = Color.green;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_Material.color = Color.red;
            audio.Play();
        }
        else
        {

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_Material.color = Color.green;
            audio.Stop();
        }
        else
        {

        }
    }

   
}
