using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tanksonduty : MonoBehaviour
{
    // Start is called before the first frame update
    public float onduty = 4;
    public float alive = 5;

    public void tankup()
    {
        onduty++;
    }
    public void tankdown()
    {
        onduty--;
    }
    public float tankreceive()
    {
        return onduty;
    }
    public void tankdead()
    {
        alive--;
    }
    public float aliveran()
    {
        return alive;
    }
}
