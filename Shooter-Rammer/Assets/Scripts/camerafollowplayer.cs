using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerafollowplayer : MonoBehaviour
{
    // Start is called before the first frame update
    public float cameraDistOffset = 10;
    public float cameraDistOffset2 = 10;
    public float cameraDistOffset3 = 10;
    private Camera mainCamera;
    public GameObject player;
    public GameObject ground;
    // Use this for initialization
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraDistOffset3 = player.transform.position.y - ground.transform.position.y;
        mainCamera.transform.position = new Vector3(player.transform.position.x - cameraDistOffset, 200 + cameraDistOffset3, player.transform.position.z - cameraDistOffset2);
       mainCamera.transform.rotation = Quaternion.Euler(new Vector3(49.024f,180, -0.028f));  // 90 degress on the X axis - change appropriately
    }
}
