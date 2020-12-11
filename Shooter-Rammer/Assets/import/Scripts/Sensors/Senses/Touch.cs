using UnityEngine;
using System.Collections;

public class Touch : Sense
{
    public GameObject parentTank;
    void OnTriggerEnter(Collider other)
    {
        Aspect aspect = other.GetComponent<Aspect>();
        if (aspect != null)
        {
            //Check the aspect
            if (aspect.aspectName == aspectName)
            {
                if(parentTank.GetComponent<NPCTankController>().fastat)
                {

                }
                else if (!parentTank.GetComponent<NPCTankController>().fastat)
                {
                    parentTank.GetComponent<NPCTankController>().touch_player = true;
                }
                Debug.Log("Enemy Touch Detected");
            }
        }
    }


}
