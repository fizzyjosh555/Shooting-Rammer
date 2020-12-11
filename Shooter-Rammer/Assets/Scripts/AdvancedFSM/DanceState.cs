using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceState : FSMState
{
    

    public DanceState(Transform[] wp)
    {
        waypoints = wp;
        stateID = FSMStateID.Dance;

        curRotSpeed = 1.0f;
        curSpeed = 0.0f;
        timer = 0.0f;
        
        //find next Waypoint position
        FindNextPoint();
    }

    public override void Reason(Transform player, Transform npc)
    {
        //Set the target position to the npc position
        destPos = npc.position;
        

        //Check the distance with player tank
        //When the distance is near, transition to chase state
        float dist = Vector3.Distance(npc.position, destPos);
        if (Vector3.Distance(npc.position, player.position) <= 300.0f)
        {
            timer = 0.0f;
            Debug.Log("Switch to Chase State");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.SawPlayer);
            npc.GetComponent<NPCTankController>().m_Material.color = Color.yellow;
        }
        else if (timer >= 10.0f)
        {
            timer = 0.0f;
            Debug.Log("Switch to patrol State");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.dancedone);
            npc.GetComponent<NPCTankController>().m_Material.color = Color.green;
        }
        else if (npc.GetComponent<NPCTankController>().health <= 50)
        {
            Debug.Log("Switch to Healing state");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.LowHealth);
            npc.GetComponent<NPCTankController>().m_Material.color = Color.red;
        }
        else
        {
            timer += Time.deltaTime;
            npc.GetComponent<NPCTankController>().m_Material.color = Color.magenta;
        }
        
    }

    public override void Act(Transform player, Transform npc)
    {
        destPos = npc.position * Time.deltaTime;


        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        npc.GetComponent<NPCTankController>().navagent.destination = destPos;
        
    }

    

}
