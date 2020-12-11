using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBox : MonoBehaviour
{
    public GameObject[] Enemies;
    public GameObject coinBoxCollider;
    public int enemyidentifier;
    private Animator coinBoxAnim;
    
    // Start is called before the first frame update
    void Start()
    {
        coinBoxAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemies[enemyidentifier] != null)
        {
            
        }
        else
        {
            coinBoxAnim.SetTrigger("OpenBox");
            Debug.Log(Enemies.Length);
            coinBoxCollider.SetActive(false);

        }
    }
}
