using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffState : FSMState
{
    // Start is called before the first frame update

    public OffState(Transform[] wp)
    {
        waypoints = wp;
        stateID = FSMStateID.dayover;


        timer = 0.0f;
        curRotSpeed = 1.0f;
        curSpeed = 0.0f;
        FindNextPoint();

    }

    public override void Reason(Transform player, Transform npc)
    {
        if (timerduty >= npc.GetComponent<NPCTankController>().timecheckdutyoff)
        {
            destPos = new Vector3(0, 0, 0);
            timerduty = 0.0f;
            curSpeed = 100.0f;
            npc.GetComponent<NPCTankController>().worktime = Random.Range(45.0f, 60.0f);
            Debug.Log("Switch to Healing state");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.daystart);
            npc.GetComponent<NPCTankController>().m_Material.color = Color.green;
            npc.GetComponent<NPCTankController>().Tankcheck.tankup();
        }
        else
        {
            
            timerduty += Time.deltaTime;
            Debug.Log("timerduty: " + timerduty.ToString());
            npc.GetComponent<NPCTankController>().m_Material.color = Color.clear;
        }


    }

    public override void Act(Transform player, Transform npc)
    {
        destPos = npc.position;
        npc.GetComponent<NPCTankController>().navagent.destination = destPos;
        if(npc.position == destPos)
        {
            npc.transform.RotateAroundLocal(new Vector3  (0 ,1.0f, 0), 10.0f);
        }
        else
        {
            
        }
    }
}
